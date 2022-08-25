using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class scr_StarGoalUpdater : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI OneStarText, TwoStarText, ThreeStarText;

    private void Awake()
    {
        OneStarText.text = "Complete in " + scr_GameManager.instance.OneStarMin.ToString() + " moves";
        TwoStarText.text = "Complete in " + scr_GameManager.instance.TwoStarMin.ToString() + " moves";
        ThreeStarText.text = "Complete in " + scr_GameManager.instance.ThreeStarMin.ToString() + " moves";
    }

}
