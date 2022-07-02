using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scr_BoardCheck : MonoBehaviour
{
    public static scr_BoardCheck ins { get; private set; }
    bool finishUpdating = false;

    private void Awake()
    {
        if (ins == null)
        {
            ins = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(this);
        }
    }

    private void Update()
    {
        if (finishUpdating)
        {
            if (!Input.GetMouseButton(0))
            {
                scr_GameManager.instance.b_UpdatingBoard = false;
                finishUpdating = false;
            }
        }
    }

    public void StartUpdating()
    {
        if (!scr_GameManager.instance.b_UpdatingBoard)
        {
            scr_GameManager.instance.b_UpdatingBoard = true;
        }
    }

    public void FinishUpdating()
    {
        finishUpdating = true;
    }
}
