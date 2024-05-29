using System;
using System.Collections;
using System.Collections.Generic;
using RelayComputer;
using UnityEngine;

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
}
