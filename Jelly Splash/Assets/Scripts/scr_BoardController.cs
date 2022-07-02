using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class scr_BoardController : MonoBehaviour
{
    //Reference Global Manager Instance
    [SerializeField] private scr_GameManager ManagerInstance = scr_GameManager.instance;

    [SerializeField] private Slider WidthSlider, HeightSlider;
    [SerializeField] private TextMeshProUGUI WidthText, HeightText;

    private void Awake()
    {
        if (ManagerInstance == null)
        {
            ManagerInstance = FindObjectOfType<scr_GameManager>();
            if (WidthSlider)
            {
                ManagerInstance.m_BoardWidth = (int)WidthSlider.value;
            }
            else
            {
                ManagerInstance.m_BoardHeight = (int)HeightSlider.value;
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        if (WidthSlider != null)
        {
            WidthSlider.onValueChanged.AddListener((v) =>
            {
                WidthText.text = v.ToString("0");
                if (ManagerInstance)
                {
                    ManagerInstance.m_BoardWidth = (int)WidthSlider.value;
                }
            });
        }
        else
        {
            HeightSlider.onValueChanged.AddListener((v) =>
            {
                HeightText.text = v.ToString("0");
                if (ManagerInstance)
                {
                    ManagerInstance.m_BoardHeight = (int)HeightSlider.value;
                }
            });
        }
    }

}
