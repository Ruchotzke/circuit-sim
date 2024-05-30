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

        /// <summary>
        /// Merge two nodes together (directly connect them).
        /// </summary>
        /// <param name="other"></param>
        public void MergeWith(Node other)
        {
            if (other == this) return;

            foreach (var component in other.ConnectedComponents)
            {
                if (!ConnectedComponents.Contains(component))
                {
                    ConnectedComponents.Add(component);
                }
                component.RemapComponent(other, this);
            }
        }
    }
}
