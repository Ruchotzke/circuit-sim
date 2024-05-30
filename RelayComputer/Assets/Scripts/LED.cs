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
    
    /// <summary>
    /// Convert nodes during a merge.
    /// </summary>
    /// <param name="from"></param>
    /// <param name="to"></param>
    public void RelinkNode(CircuitNode from, CircuitNode to)
    {
        if (InputPin.Node == from) InputPin.Node = to;
        if (OutputPin.Node == from) OutputPin.Node = to;
    }
    
    /// <summary>
    /// Get the opposing side of this component.
    /// </summary>
    /// <param name="source"></param>
    /// <returns></returns>
    public CircuitNode GetOpposing(CircuitNode source)
    {
        if (InputPin.Node == OutputPin.Node) return null; // avoid infinite loops
        if (InputPin.Node == source) return OutputPin.Node;
        if (OutputPin.Node == source) return InputPin.Node;
            
        return null;
    }
}
