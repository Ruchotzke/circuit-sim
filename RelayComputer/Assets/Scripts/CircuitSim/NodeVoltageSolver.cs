using System.Collections.Generic;
using Matrices;

namespace CircuitSim
{
    public class NodeVoltageSolver
    {
        /// <summary>
        /// Use the node-voltage method to solve nodes.
        /// </summary>
        /// <param name="nodes"></param>
        /// <returns>A parallel list of voltages for nodes.</returns>
        public static List<float> Solve(List<Node> nodes, Node ground)
        {
            /* First, create a list to work through the nodes with, starting at highest voltages */
            List<Node> open = new List<Node>();
            List<Node> closed = new List<Node>();
            foreach (var node in nodes)
            {
                foreach (var component in node.ConnectedComponents)
                {
                    if (component.IsVoltageSource) open.Add(node);
                    break;
                }
            }
            
            /* Create an output matrix of the correct size */
            Matrix system = Matrix.Zeroes(((uint rows, uint cols)) (nodes.Count, nodes.Count));
            Matrix voltages = Matrix.Zeroes(((uint rows, uint cols)) (nodes.Count, 1));
            
            /* Iterate through each node and work on solving */
            while (open.Count > 0)
            {
                /* Get the next element */
                var next = open[0];
                uint index = (uint) nodes.IndexOf(next);
                open.RemoveAt(0);
                closed.Add(next);
                
                /* if this node is ground, the voltage is always zero */
                bool isSolved = false;
                if (ground == next)
                {
                    system[index, index] = 1;
                    isSolved = true;
                }
                
                /* first, if this node contains a source, it is solved */
                if (!isSolved)
                {
                    foreach (var component in next.ConnectedComponents)
                    {
                        if (component.IsVoltageSource)
                        {
                            voltages[index, 0] = component.SourceVoltage;
                            system[index, index] = 1;

                            isSolved = true;
                            break;
                        }
                    
                    } 
                }

                /* If this node is not solved, work through each other component and set up the rest of the system */
                foreach (var component in next.ConnectedComponents)
                {
                    if (!component.IsVoltageSource)
                    {
                        /* Add the other side to the open set, if not already there */
                        var other = component.GetOpposingSide(next);
                        if (!open.Contains(other) && !closed.Contains(other))
                        {
                            open.Add(other);
                        }
                        
                        /* Update the node voltage equation if this node needs to be solved */
                        if (!isSolved)
                        {
                            uint otherIndex = (uint) nodes.IndexOf(other);
                            system[index, otherIndex] += 1 / component.Impedance;
                            system[index, index] -= 1 / component.Impedance;
                        }
                    }
                }
            }
            
            /* Solve the system of equations */
            Matrix nodeVoltages = system.Solve(voltages);
                
            /* Convert the result into array form for easy access */
            List<float> ret = new List<float>();
            for (uint i = 0; i < nodes.Count; i++)
            {
                ret.Add(nodeVoltages[i, 0]);
            }

            return ret;
        }
    }
}