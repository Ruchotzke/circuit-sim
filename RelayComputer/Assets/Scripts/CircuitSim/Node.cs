using System.Collections.Generic;
using UnityEngine;

namespace CircuitSim
{
    /// <summary>
    /// A node (interconnection of multiple components).
    /// </summary>
    public class Node
    {
        public List<Component> ConnectedComponents;

        public Node()
        {
            ConnectedComponents = new List<Component>();
        }
    }
}
