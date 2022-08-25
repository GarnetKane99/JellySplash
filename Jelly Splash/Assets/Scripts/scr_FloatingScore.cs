using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class scr_FloatingScore : MonoBehaviour
{
    float Timer = 0;
    float MaxTimer = 1.5f;
    public bool StartingLerp = false;
    [SerializeField] private TextMeshProUGUI ScoreText;

    // Update is called once per frame
    void Update()
    {
        if (StartingLerp)
        {
            Lerp();
        }
        else
        {
            Timer = 0.0f;
        }
    }

    void Lerp()
    {
        Timer += Time.deltaTime;
        if (Timer < MaxTimer)
        {
            transform.position = Vector3.Lerp(transform.position, new Vector3(transform.position.x, transform.position.y + 0.1f, transform.position.z), Timer);
        }
        else
        {
            ScoreText.text = "";
            StartingLerp = false;
        }
    }
}
