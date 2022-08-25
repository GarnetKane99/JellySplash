using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class scr_GameManager : MonoBehaviour
{
    public static scr_GameManager instance { get; private set; }

    //Define Game board size
    [Header("Board Properties")]
    public int m_BoardWidth;
    public int m_BoardHeight;
    public int[,] m_BoardLayout;

    [Header("Cell Properties")] //The number of cells can be changed to control how many *different* tiles will be spawned
    [Tooltip("Minimum range of tiles that will be needed")]
    public int m_MinObject;
    [Tooltip("Maximum range of tiles -EXCLUSIVE- that will be needed")]
    [Range(0, 5)]
    public int m_MaxObject;
    public int m_MinSelected = 3;
    public int m_CurrentSelected = 0;
    public int m_HighlightedCell = 10;

    public List<GameObject> g_SpriteObjects;
    public List<Sprite> g_AwakeSpriteAlternates;

    public List<scr_AnimationClips> g_AnimClips;

    public bool b_UpdatingBoard = false;

    public int CurrentScore;
    public int ScoreIncrease;
    public int ScoreMultiplier;

    public scr_CellLogic currentCellSetup;

    public string LevelObjectiveTitle, LevelObjectiveDesc;

    //Level Specific Details
    [Header("Level Details")]
    public bool RegularMode = false;
    public bool LevelComplete = false;
    public LevelType _LevelType;
    public int CurMoves = 0;
    public int MaxMoves = 0;
    public int MinCombo = 0;
    public int MinScore = 0;
    public int CurLevel = 1;
    public float LevelTimer = 0;
    public int CurCombo = 0;
    public int ThreeStarMin, TwoStarMin, OneStarMin;

    public int ComboText;

    public int TotalStarsForLevel = 0;
    public Animator TransitionAnim;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(this);
        }
        m_BoardWidth = 5;
        m_BoardHeight = 5;
        m_MaxObject = 5;
    }

    private void Update()
    {
        LevelTimer -= Time.deltaTime;

        if (RegularMode && !b_UpdatingBoard)
        {
            switch (_LevelType)
            {
                //Must beat total moves and score must be greater than needed score
                case LevelType.MaxMoves:
                    if (CurMoves <= MaxMoves && CurrentScore >= MinScore)
                    {
                        FinishLevel();
                    }
                    else if (CurMoves > MaxMoves && CurrentScore < MinScore)
                    {
                        FailLevel();
                    }
                    break;
                //Infinite time
                case LevelType.MinScore:
                    if (CurrentScore >= MinScore)// && LevelTimer > 0)
                    {
                        FinishLevel();
                    }
                    break;
                //Infinite time to get combo
                case LevelType.MinCombo:
                    if (CurCombo >= MinCombo)
                    {
                        FinishLevel();
                    }
                    break;
                //Must beat minscore and timer must be greater than 0
                case LevelType.GameTimer:
                    if (CurrentScore >= MinScore && LevelTimer > 0)
                    {
                        FinishLevel();
                    }
                    else if (CurrentScore < MinScore && LevelTimer <= 0)
                    {
                        FailLevel();
                    }
                    break;
            }
        }
    }

    private void FailLevel()
    {
        scr_UserInput.GetInput -= currentCellSetup.GetUserInput;
        scr_UserInput.RemoveInput -= currentCellSetup.RemoveInput;
        LevelComplete = false;
        RegularMode = false;

        //scr_LevelManager.instance.StageCount[CurLevel - 1] = true;
        //scr_LevelManager.instance.StarsGainedPerStage[CurLevel - 1] = TotalStarsForLevel;
        scr_LevelManager.instance.LevelFailed = true;
        scr_LevelManager.instance.CurLevel = CurLevel;
        scr_MenuHandler.ins.TransitionAnim = TransitionAnim;
        scr_MenuHandler.ins.Invoke("PreRegularMode", 0.5f);
    }

    private void FinishLevel()
    {
        scr_UserInput.GetInput -= currentCellSetup.GetUserInput;
        scr_UserInput.RemoveInput -= currentCellSetup.RemoveInput;
        LevelComplete = true;
        RegularMode = false;

        //This will be changed eventually so that stars gained are different depending on how level is played
        //TotalStarsForLevel = 3;
        if (CurMoves <= ThreeStarMin)
        {
            TotalStarsForLevel = 3;
        }
        else if (CurMoves <= TwoStarMin)
        {
            TotalStarsForLevel = 2;
        }
        else if (CurMoves <= OneStarMin)
        {
            TotalStarsForLevel = 1;
        }
        else
        {
            TotalStarsForLevel = 0;
        }

        scr_LevelManager.instance.StageCount[CurLevel - 1] = true;
        if (scr_LevelManager.instance.LevelScore[CurLevel - 1] < CurrentScore)
        {
            scr_LevelManager.instance.LevelScore[CurLevel - 1] = CurrentScore;
        }
        if (scr_LevelManager.instance.StarsGainedPerStage[CurLevel - 1] < TotalStarsForLevel)
        {
            scr_LevelManager.instance.StarsGainedPerStage[CurLevel - 1] = TotalStarsForLevel;
        }
        scr_LevelManager.instance.CurLevel = CurLevel;
        scr_MenuHandler.ins.TransitionAnim = TransitionAnim;
        scr_MenuHandler.ins.Invoke("PreRegularMode", 0.5f);
    }
}
