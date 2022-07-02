using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scr_ButtonFinder : MonoBehaviour
{
    public string methodName;

    public void ButtonClick()
    {
        scr_MenuHandler.ins.Invoke(methodName, 0);
    }
}
