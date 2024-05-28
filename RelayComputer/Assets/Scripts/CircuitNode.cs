using System.Collections.Generic;
using RelayComputer;

/// <summary>
/// A node in the high-level circuit.
/// </summary>
public class CircuitNode
{
    public List<Relay> Relays;
    public List<Switch> Switches;
    public List<LED> Leds;

    public bool IsSource;
    public bool IsGround;
    
    public CircuitNode(bool isGround, bool isSource)
    {
        Relays = new List<Relay>();
        Switches = new List<Switch>();
        Leds = new List<LED>();

        IsSource = isSource;
        IsGround = isGround;
    }

    /// <summary>
    /// Merge this circuit node with another, combining all elements into this node.
    /// </summary>
    /// <param name="other"></param>
    public void Merge(CircuitNode other)
    {
        foreach (var item in other.Relays)
        {
            if (!Relays.Contains(item))
            {
                Relays.Add(item);
            }
        }
        
        foreach (var item in other.Switches)
        {
            if (!Switches.Contains(item))
            {
                Switches.Add(item);
            }
        }
        
        foreach (var item in other.Leds)
        {
            if (!Leds.Contains(item))
            {
                Leds.Add(item);
            }
        }
    }
}