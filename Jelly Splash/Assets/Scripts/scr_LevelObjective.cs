using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class scr_LevelObjective : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI LevelObjectiveTitle;
    [SerializeField] private TextMeshProUGUI LevelObjectiveDescription;
    [SerializeField] private TextMeshProUGUI LevelText;
    [SerializeField] private TextMeshProUGUI LevelNum;

    private void Awake()
    {
        LevelObjectiveTitle.text = scr_GameManager.instance.LevelObjectiveTitle.ToUpper();
        LevelObjectiveDescription.text = scr_GameManager.instance.LevelObjectiveDesc;
        LevelNum.text = "";
        switch (scr_GameManager.instance._LevelType)
        {
            case LevelType.GameTimer:
                LevelText.text = "Time Left:";
                break;
            case LevelType.MaxMoves:
                LevelText.text = "Moves Left:";
                break;
            case LevelType.MinCombo:
                LevelText.text = "Current Combo:";
                break;
            case LevelType.MinScore:
                LevelText.text = "Score needed:";
                break;
        }
    }

    private void Update()
    {
        switch (scr_GameManager.instance._LevelType)
        {
            case LevelType.GameTimer:
                LevelNum.text = scr_GameManager.instance.LevelTimer.ToString();
                break;
            case LevelType.MaxMoves:
                LevelNum.text = (scr_GameManager.instance.MaxMoves - scr_GameManager.instance.CurMoves).ToString();
                break;
            case LevelType.MinCombo:
                LevelNum.text = scr_GameManager.instance.ComboText.ToString();
                break;
            case LevelType.MinScore:
                LevelNum.text = (scr_GameManager.instance.MinScore - scr_GameManager.instance.CurrentScore).ToString();
                break;
        }
    }
}
