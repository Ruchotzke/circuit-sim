using System;
using System.Collections;
using System.Collections.Generic;
using RelayComputer;
using UnityEngine;
using UnityEngine.Serialization;

public class Switch : MonoBehaviour, IInteractable
{
    [Header("Colors")]
    public Color OffColor;
    public Color OnColor;

    [Header("Components")] 
    public SpriteRenderer Button;

    [Header("State")] 
    public bool IsEnabled;

    public CircuitPin NCPin;
    public CircuitPin NOPin;
    public CircuitPin OutPin;

    private void Awake()
    {
        Button.color = IsEnabled ? OnColor : OffColor;
        
        /* Generate pins */
        NCPin.ParentComponent = gameObject;
        NCPin.Node = new CircuitNode(false, false);
        NCPin.Node.Switches.Add(this);
        NOPin.ParentComponent = gameObject;
        NOPin.Node = new CircuitNode(false, false);
        NOPin.Node.Switches.Add(this);
        OutPin.ParentComponent = gameObject;
        OutPin.Node = new CircuitNode(false, false);
        OutPin.Node.Switches.Add(this);
    }

    public void Toggle()
    {
        IsEnabled = !IsEnabled;
        Button.color = IsEnabled ? OnColor : OffColor;
    }

    public void OnInteract(Vector2 position)
    {
        Toggle();
    }
    
    /// <summary>
    /// Convert nodes during a merge.
    /// </summary>
    /// <param name="from"></param>
    /// <param name="to"></param>
    public void RelinkNode(CircuitNode from, CircuitNode to)
    {
        if (NCPin.Node == from) NCPin.Node = to;
        if (NOPin.Node == from) NOPin.Node = to;
        if (OutPin.Node == from) OutPin.Node = to;
    }

    /// <summary>
    /// Get the opposing side of this component.
    /// </summary>
    /// <param name="source"></param>
    /// <returns></returns>
    public CircuitNode GetOpposing(CircuitNode source)
    {
        if (IsEnabled)
        {
            if (OutPin.Node == NOPin.Node) return null; //avoid infinite loops
            if (OutPin.Node == source) return NOPin.Node;
            if (NOPin.Node == source) return OutPin.Node;
        }
        else
        {
            if (OutPin.Node == NCPin.Node) return null; //avoid infinite loops
            if (OutPin.Node == source) return NCPin.Node;
            if (NCPin.Node == source) return OutPin.Node;
        }

        return null;
    }
}
