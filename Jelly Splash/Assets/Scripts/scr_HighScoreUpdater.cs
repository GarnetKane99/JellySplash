using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class scr_HighScoreUpdater : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI HighScoreText;

    private void Awake()
    {
        HighScoreText.text = scr_LevelManager.instance.LevelScore[scr_LevelManager.instance.CurLevel].ToString();
    }

    private void Update()
    {
        if(scr_GameManager.instance.CurrentScore > int.Parse(HighScoreText.text))
        {
            HighScoreText.text = scr_GameManager.instance.CurrentScore.ToString();
        }
    }
}
