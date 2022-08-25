using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class scr_LevelManager : MonoBehaviour
{
    public static scr_LevelManager instance { get; private set; }

    public int[] StarsGainedPerStage;
    public int[] LevelScore;
    public bool[] StageCount;
    public List<scr_LevelDescription> Levels;
    public RectTransform StageLayout;
    public bool UpdatingScene = false;

    [SerializeField] private float Timer = 0.0f;
    [SerializeField] private float MaxTimer = 3.0f;
    public int CurLevel = 0;
    public int MaxLevel = 0;

    public bool LevelFailed = false;

    public bool HasCalled = false;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        if (UpdatingScene)
        {
            Timer += Time.deltaTime;
            if (Timer < MaxTimer)
            {
                StageLayout.localPosition = Vector3.Lerp(StageLayout.localPosition, new Vector3((-800) * CurLevel, StageLayout.localPosition.y, StageLayout.localPosition.z), Timer * Time.deltaTime);
            }
            else
            {
                Levels[CurLevel].Play.SetActive(true);
                Levels[CurLevel].Back.SetActive(true);
                StageLayout.localPosition = new Vector3((-800) * CurLevel, StageLayout.localPosition.y, StageLayout.localPosition.z);
                UpdatingScene = false;
            }
        }
    }

    // Start is called before the first frame update
    public void ResetScene()
    {
        StageLayout.localPosition = new Vector3(-800 * (CurLevel - 1), StageLayout.localPosition.y, StageLayout.localPosition.z);

        if (LevelFailed)
        {
            for (int i = 0; i < StageCount.Length; i++)
            {
                if (StageCount[i] == true)
                {
                    Levels[i].LevelEnabled = true;
                    Levels[i].HighScore = LevelScore[i];
                    Levels[i].Play.SetActive(false);
                    Levels[i].Back.SetActive(false);
                    Levels[i].StarCount = StarsGainedPerStage[i];
                    Levels[i].AutoComplete(false);
                }
            }

            Levels[CurLevel - 1].LevelFailed.SetActive(true);
            LevelFailed = false;
            CurLevel -= 1;
            UpdateLevelVisuals();
            return;
        }
        else if (scr_GameManager.instance.LevelComplete && CurLevel > MaxLevel)
        {
            for (int i = 0; i < StageCount.Length - 1; i++)
            {
                if (StageCount[i + 1] == true)
                {
                    Levels[i].LevelEnabled = true;
                    Levels[i].Play.SetActive(false);
                    Levels[i].Back.SetActive(false);
                    Levels[i].StarCount = StarsGainedPerStage[i];
                    Levels[i].HighScore = LevelScore[i];
                    Levels[i].AutoComplete(false);
                }
            }

            Levels[CurLevel - 1].LevelEnabled = true;
            Levels[CurLevel - 1].Play.SetActive(false);
            Levels[CurLevel - 1].Back.SetActive(false);
            Levels[CurLevel - 1].StarCount = StarsGainedPerStage[CurLevel - 1];
            Levels[CurLevel - 1].HighScore = LevelScore[CurLevel - 1];
            Levels[CurLevel - 1].AutoComplete(true);
        }
        else
        {
            FinishUpdate();
            for (int i = 0; i < StageCount.Length; i++)
            {
                if (StageCount[i] == true)
                {
                    Levels[i].LevelEnabled = true;
                    Levels[i].Play.SetActive(false);
                    Levels[i].Back.SetActive(false);
                    Levels[i].StarCount = StarsGainedPerStage[i];
                    Levels[i].HighScore = LevelScore[i];
                    Levels[i].AutoComplete(false);
                }
            }
        }
    }

    private void UpdateLevelVisuals()
    {
        Levels[CurLevel].LevelEnabled = true;
        Levels[CurLevel].Play.SetActive(true);
        Levels[CurLevel].Back.SetActive(true);
    }

    public void FinishUpdate()
    {
        Timer = 0.0f;
        scr_GameManager.instance.LevelComplete = false;

        if (CurLevel < Levels.Count)
        {
            UpdatingScene = true;
            Levels[CurLevel].LevelEnabled = true;
        }
        else
        {
            CurLevel = CurLevel - 1;
            FinishUpdate();
            return;
        }
        if (CurLevel > MaxLevel)
        {
            MaxLevel = CurLevel;
        }
    }
}
