using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class scr_MenuHandler : MonoBehaviour
{
    public static scr_MenuHandler ins { get; private set; }
    public Animator TransitionAnim;

    scr_CellLogic currentCellSetup = null;

    private void Awake()
    {
        if (SceneManager.GetActiveScene().name == "sc_MainMenu")
        {
            scr_GameManager.instance.CurrentScore = 0;
            scr_GameManager.instance.b_UpdatingBoard = false;
            scr_GameManager.instance.m_BoardWidth = 5;
            scr_GameManager.instance.m_BoardHeight = 5;
            scr_GameManager.instance.m_MaxObject = 5;
            scr_GameManager.instance.RegularMode = false;
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
        if (currentCellSetup == null)
        {
            currentCellSetup = FindObjectOfType<scr_CellLogic>();
        }
    }

    public void PreRegularMode()
    {
        if (scr_LevelManager.instance != null)
        {
            scr_LevelManager.instance.Invoke("ResetScene", 1.5f);
        }

        StartCoroutine(LoadScene("sc_pre_RegularMode"));
    }

    public void RegularMode()
    {
        StartCoroutine(LoadScene("sc_RegularMode"));
    }

    public void Endless()
    {
        StartCoroutine(LoadScene("sc_MainLevel"));
    }

    public void EndlessMode()
    {
        StartCoroutine(LoadScene("sc_pre_EndlessMode"));
    }

    private IEnumerator LoadScene(string SceneToLoad)
    {
        TransitionAnim.SetTrigger("Transition");
        yield return new WaitForSeconds(1.25f);
        SceneManager.LoadScene(SceneToLoad);
    }

    public void ReturnToMenu()
    {
        if(scr_LevelManager.instance != null)
        {
            scr_LevelManager.instance.CurLevel = scr_LevelManager.instance.MaxLevel;
        }

        scr_UserInput.GetInput -= currentCellSetup.GetUserInput;
        scr_UserInput.RemoveInput -= currentCellSetup.RemoveInput;
        StartCoroutine(LoadScene("sc_MainMenu"));
    }

    public void Back()
    {
        if (scr_LevelManager.instance != null)
        {
            scr_LevelManager.instance.CurLevel = scr_LevelManager.instance.MaxLevel;
        }

        scr_GameManager.instance.LevelComplete = false;
        StartCoroutine(LoadScene("sc_MainMenu"));
    }
}
