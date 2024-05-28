using System;
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

    public CircuitNode Input;
    public CircuitNode Output;

    public bool IsOn = false;

    private void Awake()
    {
        /* Initialize the pins to their own nodes */
        Input = new CircuitNode(false, false);
        Output = new CircuitNode(false, false);
    }

    public void UpdateGraphics()
    {
        Bulb.color = IsOn ? OnColor : OffColor;
    }
}
