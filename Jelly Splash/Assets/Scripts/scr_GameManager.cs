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
    [Range(0,5)]
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
    }
}
