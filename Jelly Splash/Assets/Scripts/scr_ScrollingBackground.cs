using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class scr_ScrollingBackground : MonoBehaviour
{
    public RawImage g_ScrollingImage;
    public float xSpeed, ySpeed;

    // Update is called once per frame
    void Update()
    {
        g_ScrollingImage.uvRect = new Rect(g_ScrollingImage.uvRect.position + new Vector2(xSpeed, ySpeed) * Time.deltaTime, g_ScrollingImage.uvRect.size);
    }
}
