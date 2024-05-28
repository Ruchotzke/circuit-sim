using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircuitManager : MonoBehaviour
{

    /* Singleton */
    /// <summary>
    /// The singleton reference for the circuit manager.
    /// </summary>
    public static CircuitManager Instance;

    [Header("Wires")] 
    public Wire PositiveRail;
    public Wire GroundRail;
    
    void Awake()
    {
        /* Configure Singleton */
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        /* Configure initial nodes */
        PositiveRail.Node = new CircuitNode(false, true);
        GroundRail.Node = new CircuitNode(true, false);
    }
}
