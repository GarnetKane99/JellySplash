using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement;

public class scr_CellSetup : MonoBehaviour
{
    //Reference Global Manager Instance
    public scr_GameManager ManagerInstance = scr_GameManager.instance;

    [Header("CellProperties")]
    public int StartingPiece = int.MaxValue;
    public int NextPiece = int.MaxValue;
    public Vector2Int StartingVectorCoordinate;
    public Vector2Int NextVectorCoordinate;

    [Header("Used Locations")]
    [Tooltip("This is so that we can track which locations have been selected and not reselect positions")]
    [SerializeField] public List<Vector2Int> FoundCoordinates;

    private bool b_HasSetup = false;
    [HideInInspector] public bool b_UpdatingBoard = false;
    [HideInInspector] public float sqrRadius = 0.125f;
    public scr_CellOwner[,] CellScripts;

    public float t_TimeBetweenPops;
    public float t_TimeBetweenTransition;

    private GameObject Grid1, Grid2;

    private void Awake()
    {
        Grid1 = Resources.Load("Prefabs/BugJoinGrid_0") as GameObject;
        Grid2 = Resources.Load("Prefabs/BugJoinGrid_1") as GameObject;

        if (SceneManager.GetActiveScene().name == "sc_MainLevel")
        {
            if (ManagerInstance == null)
            {
                ManagerInstance = FindObjectOfType<scr_GameManager>();
            }

            if (ManagerInstance != null)
            {
                ManagerInstance.m_BoardLayout = new int[ManagerInstance.m_BoardWidth, ManagerInstance.m_BoardHeight];
                CellScripts = new scr_CellOwner[ManagerInstance.m_BoardWidth, ManagerInstance.m_BoardHeight];
                CreateCells();
            }
        }
    }

    void CreateCells()
    {
        if (!b_HasSetup) //Check if it has been setup already
        {
            InitializeCells(); //if not then it will set up

            GameObject ParentObject = new GameObject();
            ParentObject.name = "*Cell Parent*";
            ParentObject.transform.parent = transform;

            GameObject GridParent = new GameObject();
            GridParent.name = "*Grid Parent*";
            GridParent.transform.parent = transform;

            int Pos = 0;
            for (int x = 0; x < ManagerInstance.m_BoardLayout.GetLength(0); x++)
            {
                scr_CellOwner scr_CellToUse = null;
                for (int y = 0; y < ManagerInstance.m_BoardLayout.GetLength(1); y++)
                {
                    GameObject _Grid;
                    if (x % 2 == 0)
                    {
                        if (y % 2 == 0)
                        {
                            _Grid = Instantiate(Grid1, new Vector2(x - ManagerInstance.m_BoardWidth / 2, y - ManagerInstance.m_BoardHeight / 2), Quaternion.identity);
                        }
                        else
                        {
                            _Grid = Instantiate(Grid2, new Vector2(x - ManagerInstance.m_BoardWidth / 2, y - ManagerInstance.m_BoardHeight / 2), Quaternion.identity);
                        }
                    }
                    else
                    {
                        if (y % 2 == 0)
                        {
                            _Grid = Instantiate(Grid2, new Vector2(x - ManagerInstance.m_BoardWidth / 2, y - ManagerInstance.m_BoardHeight / 2), Quaternion.identity);
                        }
                        else
                        {
                            _Grid = Instantiate(Grid1, new Vector2(x - ManagerInstance.m_BoardWidth / 2, y - ManagerInstance.m_BoardHeight / 2), Quaternion.identity);
                        }
                    }
                    _Grid.transform.parent = GridParent.transform;

                    GameObject _GO = Instantiate(ManagerInstance.g_SpriteObjects[ManagerInstance.m_BoardLayout[x, y]], new Vector2(x - ManagerInstance.m_BoardWidth / 2, y - ManagerInstance.m_BoardHeight / 2), Quaternion.identity);
                    _GO.transform.parent = ParentObject.transform;
                    _GO.name = Pos.ToString();
                    scr_CellOwner scr_CellCreated = _GO.GetComponent<scr_CellOwner>();
                    scr_CellCreated.TrueCoordinate = new Vector2Int(x, y);
                    scr_CellCreated.FoundValue = ManagerInstance.m_BoardLayout[x, y];
                    scr_CellCreated.PositionValue = Pos;

                    if (scr_CellToUse != null)
                    {
                        scr_CellToUse.Next = scr_CellCreated;
                        scr_CellToUse.InitializeCellData();
                    }
                    if (y == ManagerInstance.m_BoardLayout.GetLength(1) - 1)
                    {
                        scr_CellCreated.Next = scr_CellCreated;
                        scr_CellCreated.InitializeCellData();
                    }

                    scr_CellToUse = scr_CellCreated;
                    CellScripts[x, y] = scr_CellCreated;

                    Pos++;
                }
            }
            GridParent.transform.position = new Vector3(0, 0, 5);
        }
    }

    void InitializeCells()
    {
        for (int x = 0; x < ManagerInstance.m_BoardLayout.GetLength(0); x++) //Loop through the board layout length in x and y and set each value to be a random number between 0 and 4 (0,1,2,3 or 4)
        {
            for (int y = 0; y < ManagerInstance.m_BoardLayout.GetLength(1); y++)
            {
                ManagerInstance.m_BoardLayout[x, y] = Random.Range(ManagerInstance.m_MinObject, ManagerInstance.m_MaxObject);
            }
        }

        //ShowMatrix();

        b_HasSetup = true;
    }

    //Show the matrix array for debugging purposes
    public void ShowMatrix()
    {
        StringBuilder sb = new StringBuilder();
        for (int y = ManagerInstance.m_BoardLayout.GetLength(1) - 1; y >= 0; y--)
        {
            for (int x = 0; x < ManagerInstance.m_BoardLayout.GetLength(0); x++)
            {
                sb.Append(ManagerInstance.m_BoardLayout[x, y]);
                sb.Append(' ');
            }
            sb.AppendLine();
        }
        Debug.Log(sb.ToString());
    }
}
