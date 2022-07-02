using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scr_UserInput : MonoBehaviour
{
    //Reference Global Manager Instance
    [SerializeField] private scr_GameManager ManagerInstance = scr_GameManager.instance;

    //Handles all user input so that user input is all done within one script
    public delegate void RetrieveUserInput(Vector2 MouseInput);
    public static event RetrieveUserInput GetInput;
    public delegate void RemoveUserInput();
    public static event RemoveUserInput RemoveInput;

    private void Awake()
    {
        if (ManagerInstance == null)
        {
            ManagerInstance = FindObjectOfType<scr_GameManager>();
        }
    }
    // Update is called once per frame
    void Update()
    {
        CheckInput();
    }

    void CheckInput()
    {
        Vector2 MousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        if (!ManagerInstance.b_UpdatingBoard)
        {
            if (Input.GetMouseButton(0))
            {
                if (GetInput != null)
                {
                    GetInput(MousePos);
                }
            }
            else
            {
                if (RemoveInput != null)
                {
                    RemoveInput();
                }
            }
        }
    }
}
