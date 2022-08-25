using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class ET_LevelCreator : EditorWindow
{
    private scr_LevelData LevelData;

    private string StageName = "";
    private string LevelDescription = "";
    private int LevelID = 1;
    private GameObject LevelToSpawn;
    private GameObject ParentObject;

    LevelType _LevelType;
    private int ScoreNeeded;
    private int MaxMoves;
    private int MinComboNeeded;
    private float GameTimer;
    public int BoardWidth;
    public int BoardHeight;
    public int BugCount;
    public int ThreeStar;
    public int TwoStar;
    public int OneStar;

    [MenuItem("Tools/Level Generator")]
    public static void ShowWindow()
    {
        GetWindow(typeof(ET_LevelCreator));
    }

    private void OnGUI()
    {
        GUILayout.Label("Spawn New Stage", EditorStyles.boldLabel);

        //Stage Data
        StageName = EditorGUILayout.TextField("Stage Name", StageName);
        //LevelDescription = EditorGUILayout.TextField("Level Description", LevelDescription);
        LevelID = EditorGUILayout.IntField("Level ID", LevelID);
        LevelToSpawn = EditorGUILayout.ObjectField("Level Prefab", LevelToSpawn, typeof(GameObject), false) as GameObject;
        ParentObject = EditorGUILayout.ObjectField("Level Parent", ParentObject, typeof(GameObject), true) as GameObject;

        //Level Data *Level INFORMATION* That gets passed to Game Manager
        _LevelType = (LevelType)EditorGUILayout.EnumPopup("Level Type", _LevelType);
        ScoreNeeded = EditorGUILayout.IntSlider("Score Needed", ScoreNeeded, 3000, 30000);
        MaxMoves = EditorGUILayout.IntSlider("Maximum Moves", MaxMoves, 5, 30);
        MinComboNeeded = EditorGUILayout.IntSlider("Combo Needed", MinComboNeeded, 5, 30);
        GameTimer = EditorGUILayout.Slider("Timer", GameTimer, 30, 120);
        ThreeStar = EditorGUILayout.IntSlider("Three Star Requirements", ThreeStar, 1, 15);
        TwoStar = EditorGUILayout.IntSlider("Two Star Requirements", TwoStar, 3, 25);
        OneStar = EditorGUILayout.IntSlider("One Star Requirements", OneStar, 5, 35);

        //Board Data
        BoardWidth = EditorGUILayout.IntSlider("Board Width", BoardWidth, 5, 10);
        BoardHeight = EditorGUILayout.IntSlider("Board Height", BoardHeight, 5, 10);
        BugCount = EditorGUILayout.IntSlider("Bug Count", BugCount, 2, 5);

        LevelData = (scr_LevelData)EditorGUILayout.ObjectField("Level Data Script", LevelData, typeof(scr_LevelData), true);

        if (GUILayout.Button("Create Level"))
        {
            SpawnObject();
        }
    }

    private void SpawnObject()
    {
        if (!LevelToSpawn || !ParentObject)
        {
            Debug.LogError("Please fill necessary fields in");
            return;
        }

        GameObject LevelSpawned = Instantiate(LevelToSpawn, new Vector3(0, 0, 0), Quaternion.identity);
        UpdateLevelDescription();
        UpdateLevelData(LevelSpawned);
    }

    private void UpdateLevelDescription()
    {
        switch (_LevelType)
        {
            case LevelType.MaxMoves:
                LevelDescription = "You have " + MaxMoves.ToString() + " moves to get a score of at least " + ScoreNeeded.ToString() + " to pass the level";
                break;
            case LevelType.MinCombo:
                LevelDescription = "You must get a combo of at least " + MinComboNeeded.ToString() + " to pass the level";
                break;
            case LevelType.MinScore:
                LevelDescription = "You need to get a score of at least " + ScoreNeeded.ToString() + " to pass the level";
                break;
            case LevelType.GameTimer:
                LevelDescription = "You must get a score of at least " + ScoreNeeded.ToString() + " before the timer runs out to pass the level";
                break;
        }
    }

    private void UpdateLevelData(GameObject LevelFound)
    {
        scr_LevelHandler LevelHandler = LevelFound.GetComponent<scr_LevelHandler>();
        scr_LevelDescription LevelDescriptionScr = LevelFound.GetComponent<scr_LevelDescription>();

        if (LevelHandler && LevelDescriptionScr)
        {
            LevelHandler.transform.parent = ParentObject.transform;
            LevelHandler.transform.localPosition = new Vector3((LevelID - 1) * 800, 0, 0);

            LevelHandler.name = StageName + " " + LevelID.ToString();
            LevelHandler._LevelType = _LevelType;
            LevelHandler.ScoreNeeded = ScoreNeeded;
            LevelHandler.MaxMoves = MaxMoves;
            LevelHandler.MinComboNeeded = MinComboNeeded;
            LevelHandler.GameTimer = GameTimer;            

            LevelHandler.BoardWidth = BoardWidth;
            LevelHandler.BoardHeight = BoardHeight;
            LevelHandler.BugCount = BugCount;

            LevelHandler.LevelNum = LevelID;

            LevelHandler.OneStarMin = OneStar;
            LevelHandler.TwoStarMin = TwoStar;
            LevelHandler.ThreeStarMin = ThreeStar;

            LevelHandler.LevelDescription.text = LevelDescription.ToUpper();
            if (LevelID < 10)
            {
                LevelDescriptionScr.StageText.text = "STAGE 00" + LevelID.ToString();
            }
            else if(LevelID < 100)
            {
                LevelDescriptionScr.StageText.text = "STAGE 0" + LevelID.ToString();
            }
            else
            {
                LevelDescriptionScr.StageText.text = "STAGE " + LevelID.ToString();
            }
            LevelDescriptionScr.LevelDescriptionText.text = LevelDescription.ToUpper();

            LevelData.Levels.Add(LevelDescriptionScr);

            LevelID++;
        }
    }
}
