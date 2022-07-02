using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class scr_MenuHandler : MonoBehaviour
{
    public static scr_MenuHandler ins { get; private set; }

    scr_CellLogic currentCellSetup = null;

    private void Awake()
    {
        if (SceneManager.GetActiveScene().name == "sc_MainMenu")
        {
            scr_GameManager.instance.CurrentScore = 0;
            scr_GameManager.instance.b_UpdatingBoard = false;
        }

        if (ins == null)
        {
            ins = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        //Debug.Log(currentCellSetup);
        if (currentCellSetup == null)
        {
            currentCellSetup = FindObjectOfType<scr_CellLogic>();
        }
    }

    public void RegularMode()
    {
        SceneManager.LoadScene("sc_MainLevel");
    }

    public void EndlessMode()
    {
        SceneManager.LoadScene("sc_pre_EndlessMode");
    }

    public void ReturnToMenu()
    {
        scr_UserInput.GetInput -= currentCellSetup.GetUserInput;
        scr_UserInput.RemoveInput -= currentCellSetup.RemoveInput;

        SceneManager.LoadScene("sc_MainMenu");
    }

    public void Back()
    {
        SceneManager.LoadScene("sc_MainMenu");
    }
}
