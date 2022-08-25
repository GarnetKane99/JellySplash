using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class scr_MoveCounter : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI MoveCounterText;
    [Header("3 Star Data")]
    [SerializeField] private Image TRStarA;
    [SerializeField] private Image TRStarB;
    [SerializeField] private Image TRStarC;
    [SerializeField] private TextMeshProUGUI ThreeStarText;
    private bool HasUpdatedThree = false;
    [Header("2 Star Data")]
    [SerializeField] private Image TStarA;
    [SerializeField] private Image TStarB;
    [SerializeField] private TextMeshProUGUI TwoStarText;
    private bool HasUpdatedTwo = false;
    [Header("1 Star Data")]
    [SerializeField] private Image OStarA;
    [SerializeField] private TextMeshProUGUI OneStarText;
    private bool HasUpdatedOne = false;

    // Update is called once per frame
    void Update()
    {
        MoveCounterText.text = scr_GameManager.instance.CurMoves.ToString();

        if(scr_GameManager.instance.CurMoves > scr_GameManager.instance.ThreeStarMin && !HasUpdatedThree)
        {
            TRStarA.color = new Color(0.5f, 0.5f, 0.5f);
            TRStarB.color = new Color(0.5f, 0.5f, 0.5f);
            TRStarC.color = new Color(0.5f, 0.5f, 0.5f);
            ThreeStarText.color = new Color(0.6603774f, 0.580834f, 0.4890531f);
            HasUpdatedThree = true; 
        }else if(scr_GameManager.instance.CurMoves > scr_GameManager.instance.TwoStarMin && !HasUpdatedTwo)
        {
            TStarA.color = new Color(0.5f, 0.5f, 0.5f);
            TStarB.color = new Color(0.5f, 0.5f, 0.5f);
            TwoStarText.color = new Color(0.6603774f, 0.580834f, 0.4890531f);
            HasUpdatedTwo = true;
        }else if(scr_GameManager.instance.CurMoves > scr_GameManager.instance.OneStarMin && !HasUpdatedOne)
        {
            OStarA.color = new Color(0.5f, 0.5f, 0.5f);
            OneStarText.color = new Color(0.6603774f, 0.580834f, 0.4890531f);
            HasUpdatedOne = true;
        }
    }
}
