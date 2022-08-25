using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class scr_BoardController : MonoBehaviour
{
    //Reference Global Manager Instance
    [SerializeField] private scr_GameManager ManagerInstance = scr_GameManager.instance;

    [SerializeField] private Slider WidthSlider, HeightSlider, BugSlider;
    [SerializeField] private TextMeshProUGUI WidthText, HeightText, BugText;

    private void Awake()
    {
        if (ManagerInstance == null)
        {
            ManagerInstance = FindObjectOfType<scr_GameManager>();

            ManagerInstance.m_BoardWidth = (int)WidthSlider.value;

            ManagerInstance.m_BoardHeight = (int)HeightSlider.value;

            ManagerInstance.m_MaxObject = (int)BugSlider.value;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        WidthSlider.onValueChanged.AddListener((v) =>
        {
            WidthText.text = v.ToString("0");
            if (ManagerInstance)
            {
                ManagerInstance.m_BoardWidth = (int)WidthSlider.value;
            }
        });

        HeightSlider.onValueChanged.AddListener((v) =>
        {
            HeightText.text = v.ToString("0");
            if (ManagerInstance)
            {
                ManagerInstance.m_BoardHeight = (int)HeightSlider.value;
            }
        });

        BugSlider.onValueChanged.AddListener((v) =>
        {
            BugText.text = v.ToString("0");
            if (ManagerInstance)
            {
                ManagerInstance.m_MaxObject = (int)BugSlider.value;
            }
        });
    }

}
