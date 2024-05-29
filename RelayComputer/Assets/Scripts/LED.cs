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

    public CircuitPin InputPin;
    public CircuitPin OutputPin;

    public bool IsOn = false;

    private void Awake()
    {
        /* Initialize the pins to their own nodes */
        InputPin.ParentComponent = gameObject;
        InputPin.Node = new CircuitNode(false, false);
        InputPin.Node.Leds.Add(this);
        OutputPin.ParentComponent = gameObject;
        OutputPin.Node = new CircuitNode(false, false);
        OutputPin.Node.Leds.Add(this);
    }

    public void UpdateGraphics()
    {
        Bulb.color = IsOn ? OnColor : OffColor;
    }
}
