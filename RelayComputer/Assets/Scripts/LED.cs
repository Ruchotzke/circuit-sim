using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LED : MonoBehaviour
{
    [Header("Colors")]
    public Color OffColor;
    public Color OnColor;

    [Header("Components")] 
    public SpriteRenderer Bulb;

    public bool IsOn = false;

    public void UpdateGraphics()
    {
        Bulb.color = IsOn ? OnColor : OffColor;
    }
}
