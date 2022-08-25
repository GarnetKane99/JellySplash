using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class scr_LevelNavigation : MonoBehaviour
{
    [SerializeField] private Button LeftClick, RightClick;

    private void Awake()
    {
        LeftClick.onClick.AddListener(GoLeft);
        RightClick.onClick.AddListener(GoRight);
    }

    private void GoLeft()
    {
        if (!scr_LevelManager.instance.UpdatingScene && !scr_GameManager.instance.LevelComplete)
        {
            if (scr_LevelManager.instance.CurLevel - 1 >= 0)
            {
                scr_LevelManager.instance.Levels[scr_LevelManager.instance.CurLevel].Play.SetActive(false);
                scr_LevelManager.instance.Levels[scr_LevelManager.instance.CurLevel].Back.SetActive(false);
                scr_LevelManager.instance.CurLevel = scr_LevelManager.instance.CurLevel - 1;
                scr_LevelManager.instance.FinishUpdate();
            }
        }
    }

    private void GoRight()
    {
        if (!scr_LevelManager.instance.UpdatingScene && !scr_GameManager.instance.LevelComplete)
        {
            if (scr_LevelManager.instance.CurLevel + 1 < scr_LevelManager.instance.Levels.Count)
            {
                if (scr_LevelManager.instance.StageCount[scr_LevelManager.instance.CurLevel] == true)
                {
                    scr_LevelManager.instance.Levels[scr_LevelManager.instance.CurLevel].Play.SetActive(false);
                    scr_LevelManager.instance.Levels[scr_LevelManager.instance.CurLevel].Back.SetActive(false);
                    scr_LevelManager.instance.CurLevel = scr_LevelManager.instance.CurLevel + 1;
                    scr_LevelManager.instance.FinishUpdate();
                }
            }
        }
    }
}
