using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class scr_LevelDescription : MonoBehaviour
{
    public bool LevelEnabled = false;

    private float Timer = 0.0f;
    private float MaxTimer = 2.0f;

    public Slider StarA, StarB, StarC;

    public scr_LevelHandler CurLevelHandler;


    bool UpdateStars = false;

    public int StarCount = 0;
    bool StarAComplete = false;
    bool StarBComplete = false;
    bool StarCComplete = false;

    [Header("Stage Properties")]
    public GameObject Play;
    public GameObject Back;
    public Image StageNum;
    //public Image StageText, StageA, StageB, StageC;
    public TextMeshProUGUI StageText;
    public Image StarABackground, StarBBackground, StarCBackground;
    public Image LevelRibbon;
    public Image LineDivider;
    public Image LevelDescriptionBackground;
    public TextMeshProUGUI LevelDescriptionHeader;
    public TextMeshProUGUI LevelDescriptionText;
    private bool HasUpdated = false;
    private int StarCounter = -1;
    private bool ChangedLayout = false;
    [SerializeField] private GameObject LevelComplete;
    public GameObject LevelFailed;

    public GameObject LevelHighScorePlacement;
    public TextMeshProUGUI HighScoreTextDisplay;

    public int HighScore;

    public void AutoComplete(bool canComplete)
    {
        if (canComplete)
        {
            UpdateStars = true;
            Timer = 0.0f;
        }
        else
        {
            switch (StarCount)
            {
                case 0:
                    break;
                case 1:
                    StarA.value = StarA.maxValue;
                    break;
                case 2:
                    StarA.value = StarA.maxValue;
                    StarB.value = StarB.maxValue;
                    break;
                case 3:
                    StarA.value = StarA.maxValue;
                    StarB.value = StarB.maxValue;
                    StarC.value = StarC.maxValue;
                    break;
                default:
                    break;
            }
        }
        LevelHighScorePlacement.SetActive(true);
        HighScoreTextDisplay.text = HighScore.ToString();
        LevelComplete.SetActive(true);
    }

    private void Update()
    {
        if (UpdateStars && !StarAComplete)
        {
            UpdateVisual(StarA);
        }
        else if (StarAComplete && !StarBComplete && StarCount > 1)
        {
            UpdateVisual(StarB);
        }
        else if (StarBComplete && !StarCComplete && StarCount > 2)
        {
            UpdateVisual(StarC);
        }
        if (scr_GameManager.instance.LevelComplete && !HasUpdated && StarCounter == StarCount)
        {
            scr_LevelManager.instance.Invoke("FinishUpdate", 0.25f);
            HasUpdated = true;
        }

        if (LevelEnabled && !ChangedLayout)
        {
            EnableLevel();
        }
    }

    private void UpdateVisual(Slider SliderToUpdate)
    {
        Timer += Time.deltaTime;

        if (Timer < MaxTimer)
        {
            SliderToUpdate.value = Mathf.Lerp(0, 5, Timer);
        }
        else if (SliderToUpdate == StarA)
        {
            StarAComplete = true;
            StarCounter = 1;
            Timer = 0.0f;
        }
        else if (SliderToUpdate == StarB)
        {
            StarBComplete = true;
            StarCounter = 2;
            Timer = 0.0f;
        }
        else if (SliderToUpdate == StarC)
        {
            StarCComplete = true;
            StarCounter = 3;
            Timer = 0.0f;
        }
    }

    private void EnableLevel()
    {
        StageText.color = new Color(1, 0.9215686f, 0.5529412f);

        StageNum.color = new Color(0.7254902f, 0.6784314f, 0.5176471f);

        StarABackground.color = new Color(1, 1, 1, 1);
        StarBBackground.color = new Color(1, 1, 1, 1);
        StarCBackground.color = new Color(1, 1, 1, 1);
        LevelRibbon.color = new Color(0.7254902f, 0.6784314f, 0.5176471f);

        LineDivider.color = new Color(1, 1, 1, 1);

        LevelDescriptionText.color = new Color(1, 0.9215686f, 0.5529412f);

        LevelDescriptionHeader.color = new Color(1, 0.9215686f, 0.5529412f);

        ChangedLayout = true;
    }
}
