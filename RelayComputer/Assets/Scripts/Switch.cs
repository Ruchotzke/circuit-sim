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

    public CircuitNode NC;
    public CircuitNode NO;
    public CircuitNode Out;

    private void Awake()
    {
        Button.color = IsEnabled ? OnColor : OffColor;
        
        /* Generate pins */
        NC = new CircuitNode(false, false);
        NO = new CircuitNode(false, false);
        Out = new CircuitNode(false, false);
    }

    public void Toggle()
    {
        IsEnabled = !IsEnabled;
        Button.color = IsEnabled ? OnColor : OffColor;
    }

    public void OnInteract()
    {
        Toggle();
    }
}
