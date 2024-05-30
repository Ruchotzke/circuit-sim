using System;
using System.Collections;
using System.Collections.Generic;
using CircuitSim;
using CircuitSim.Components;
using RelayComputer;
using UnityEngine;

public class CircuitManager : MonoBehaviour
{
    /// <summary>
    /// How many circuit iterations should be run before claiming instability.
    /// </summary>
    public const int NUM_ITERATION = 10;
    
    
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

    /// <summary>
    /// Solve the circuit to simulate it.
    /// </summary>
    private void SolveCircuit()
    {
        /* First gather all nodes */
        List<CircuitNode> nodes = new List<CircuitNode>();
        foreach (var wire in FindObjectsOfType<Wire>())
        {
            if (!nodes.Contains(wire.Node)) nodes.Add(wire.Node);
        }
        
        /* Convert all nodes into sim nodes */
        List<Node> simNodes = new List<Node>();
        Node groundNode;
        foreach (var wirenode in nodes)
        {
            wirenode.SimNode = new Node();
            simNodes.Add(wirenode.SimNode);
            if (wirenode.IsGround) groundNode = wirenode.SimNode;
        }
        
        /* Add all conductive components to their nodes */
        foreach (var led in FindObjectsOfType<LED>())
        {
            Resistor r = new Resistor(180);
            led.InputPin.Node.SimNode.ConnectedComponents.Add(r);
            led.OutputPin.Node.SimNode.ConnectedComponents.Add(r);
        }
        
        foreach (var relay in FindObjectsOfType<Relay>())
        {
            Resistor coil = new Resistor(100);
            relay.CoilAPin.Node.SimNode.ConnectedComponents.Add(coil);
            relay.CoilBPin.Node.SimNode.ConnectedComponents.Add(coil);
        }
        
        /* Based on switches, selectively merge nodes */
        List<CircuitNode> open = new List<CircuitNode>() {PositiveRail.Node};
        List<CircuitNode> closed = new List<CircuitNode>();
        while (open.Count > 0)
        {
            /* Remove the next node */
            var next = open[0];
            open.RemoveAt(0);
            closed.Add(next);
            
            /* Figure out which need to be merged with this node */
            List<CircuitNode> toMerge = new List<CircuitNode>();
            foreach(var s in next.Switches)
            {
                var opp = s.GetOpposing(next);
                if (opp != null)
                {
                    toMerge.Add(opp);
                }
            }
            
            /* Do the same with the relays */
            foreach (var r in next.Relays)
            {
                var opp = r.GetOpposingSwitch(next);
                if (opp != null)
                {
                    toMerge.Add(opp);
                }
            }
            
            /* Search over components now */
            foreach (var r in next.Relays)
            {
                var opp = r.GetOpposingCoil(next);
                if (opp != null && !closed.Contains(opp) && !open.Contains(opp))
                {
                    open.Add(opp);
                    var resistor = new Resistor(100.0f);
                    resistor.LinkT1(next.SimNode);
                    resistor.LinkT2(opp.SimNode);
                }
            }

            foreach (var l in next.Leds)
            {
                var opp = l.GetOpposing(next);
                if (opp != null && !closed.Contains(opp) && !open.Contains(opp))
                {
                    open.Add(opp);
                    var resistor = new Resistor(100.0f);
                    resistor.LinkT1(next.SimNode);
                    resistor.LinkT2(opp.SimNode);
                }
            }
            
            /* Perform the merge on nodes */
            if (toMerge.Count > 0)
            {
                foreach (var node in toMerge)
                {
                    next.SimNode.MergeWith(node.SimNode);
                    node.SimNode = next.SimNode;
                }

                open.Add(next);
            }
        }
        
        /* Add the source component to the system between + and - */
        SourceDC src = new SourceDC(10.0f);
        src.LinkT1(PositiveRail.Node.SimNode);
        src.LinkT2(GroundRail.Node.SimNode);
        
        /* Now that nodes are properly formatted for this iteration, solve the system */
        List<Node> solveList = new List<Node>();
        foreach (var cn in nodes)
        {
            if (!solveList.Contains(cn.SimNode)) solveList.Add(cn.SimNode);
        }

        var solved = NodeVoltageSolver.Solve(solveList, GroundRail.Node.SimNode);
        
        /* Add all entries into a map [node->voltage], maintaining floats as floating values */
        
        /* Check for changes between iterations, otherwise continue iterating */
    }
}
