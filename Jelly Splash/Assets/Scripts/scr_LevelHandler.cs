using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

/*This script is responsible for handling the level specific data that will then be acquired by the Game Manager to determine:
 * Level Type (MaxMoves | MinScore | MinCombo | Timer)
 * Board Width/Height
 * Bug Counts
 */

public enum LevelType
{
    MaxMoves,
    MinScore,
    MinCombo,
    GameTimer
}

public class scr_LevelHandler : MonoBehaviour
{
    public LevelType _LevelType;

    [Header("Level Details")]
    [Tooltip("Score Needed is for MinScore, MaxMoves, and Timer")]
    public int ScoreNeeded;
    [Tooltip("MaxMoves is needed for MaxMoves")]
    public int MaxMoves;
    [Tooltip("MinComboNeeded is needed for Min Combo")]
    public int MinComboNeeded;
    [Tooltip("GameTimer is needed for Timer")]
    public float GameTimer;

    [Header("Board Details")]
    public int BoardWidth;
    public int BoardHeight;
    public int BugCount;
    public int ThreeStarMin, TwoStarMin, OneStarMin;

    public TextMeshProUGUI LevelDescription;

    public int LevelNum;
}
