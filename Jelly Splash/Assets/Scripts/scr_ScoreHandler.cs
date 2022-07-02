using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class scr_ScoreHandler : MonoBehaviour
{
    //Reference Global Manager Instance
    [SerializeField] private scr_GameManager ManagerInstance = scr_GameManager.instance;

    public TextMeshProUGUI ScoreText;

    private int curScore, prevScore;

    private void Awake()
    {
        if (ManagerInstance == null)
        {
            ManagerInstance = FindObjectOfType<scr_GameManager>();
        }
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        curScore = ManagerInstance.CurrentScore;
        if (prevScore < curScore)
        {
            ScoreText.text = "00" + (prevScore += 10).ToString();
        }
    }
}
