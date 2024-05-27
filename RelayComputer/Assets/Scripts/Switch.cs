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

    private void Awake()
    {
        Button.color = IsEnabled ? OnColor : OffColor;
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
