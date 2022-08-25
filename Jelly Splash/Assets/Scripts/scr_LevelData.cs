using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scr_LevelData : MonoBehaviour
{
    public List<scr_LevelDescription> Levels;
    public RectTransform StageLayout;
    public Animator TransitionAnim;

    private void Awake()
    {
        scr_LevelManager.instance.Levels = Levels;
        scr_LevelManager.instance.StageLayout = StageLayout;

        if (!scr_LevelManager.instance.HasCalled)
        {
            LevelReset();
            scr_LevelManager.instance.HasCalled = true;
        }
        //Invoke("LevelReset", .5f);

        for (int i = 0; i < Levels.Count; i++)
        {
            Levels[i].Play.GetComponent<scr_ButtonFinder>().TransitionAnim = TransitionAnim;
            Levels[i].Back.GetComponent<scr_ButtonFinder>().TransitionAnim = TransitionAnim;
        }

        //scr_LevelManager.instance.ResetScene();

        //scr_LevelManager.instance.Levels[scr_LevelManager.instance.CurLevel].Play.SetActive(true);
        //scr_LevelManager.instance.Levels[scr_LevelManager.instance.CurLevel].Back.SetActive(true);
    }

    void LevelReset()
    {
        if (scr_LevelManager.instance.CurLevel < Levels.Count)
        {            
            scr_LevelManager.instance.Levels[scr_LevelManager.instance.CurLevel].LevelEnabled = true;
            scr_LevelManager.instance.Levels[scr_LevelManager.instance.CurLevel].Play.SetActive(true);
            scr_LevelManager.instance.Levels[scr_LevelManager.instance.CurLevel].Back.SetActive(true);
        }
    }
}
