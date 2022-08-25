using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scr_ButtonFinder : MonoBehaviour
{
    public string methodName;
    public bool RegularLevel;
    public scr_LevelHandler LevelDescType;
    public Animator TransitionAnim;

    public void ButtonClick()
    {
        scr_MenuHandler.ins.TransitionAnim = TransitionAnim;

        if (RegularLevel)
        {
            scr_GameManager GMInstance = scr_GameManager.instance;


            GMInstance.TransitionAnim = TransitionAnim;
            GMInstance.LevelComplete = false;
            GMInstance.CurCombo = 0;
            GMInstance.CurMoves = 0;
            GMInstance.RegularMode = true;
            GMInstance._LevelType = LevelDescType._LevelType;
            GMInstance.CurrentScore = 0;
            


            switch (LevelDescType._LevelType)
            {
                case LevelType.MaxMoves:
                    GMInstance.LevelObjectiveTitle = "Maximum Moves";
                    GMInstance.MaxMoves = LevelDescType.MaxMoves;
                    GMInstance.MinScore = LevelDescType.ScoreNeeded;
                    break;
                case LevelType.MinCombo:
                    GMInstance.LevelObjectiveTitle = "Minimum Combo";
                    GMInstance.MinCombo = LevelDescType.MinComboNeeded;
                    break;
                case LevelType.MinScore:
                    GMInstance.LevelObjectiveTitle = "Minimum Score";
                    GMInstance.MinScore = LevelDescType.ScoreNeeded;
                    break;
                case LevelType.GameTimer:
                    GMInstance.LevelObjectiveTitle = "Beat the Timer";
                    GMInstance.MinScore = LevelDescType.ScoreNeeded;
                    GMInstance.LevelTimer = LevelDescType.GameTimer;
                    break;
            }
            GMInstance.LevelObjectiveDesc = LevelDescType.LevelDescription.text;
            GMInstance.m_BoardWidth = LevelDescType.BoardWidth;
            GMInstance.m_BoardHeight = LevelDescType.BoardHeight;
            GMInstance.m_MaxObject = LevelDescType.BugCount;
            GMInstance.CurLevel = LevelDescType.LevelNum;
            GMInstance.ThreeStarMin = LevelDescType.ThreeStarMin;
            GMInstance.TwoStarMin = LevelDescType.TwoStarMin;
            GMInstance.OneStarMin = LevelDescType.OneStarMin;
        }

        scr_MenuHandler.ins.Invoke(methodName, 0);
    }
}
