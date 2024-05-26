using System;
using UnityEngine;

namespace CircuitSim
{
    /// <summary>
    /// A component within the circuit.
    /// </summary>
    public abstract class Component
    {
        /// <summary>
        /// Whether or not this is a source.
        /// </summary>
        public bool IsVoltageSource;

        /// <summary>
        /// The impedance of this component. Only usable for non-sources.
        /// </summary>
        public float Impedance
        {
            get
            {
                if(!IsVoltageSource) return _impedance;
                throw new ArgumentException("Cannot calculate the impedance of a source.");
            }
            set => _impedance = value;
        }

        protected float _impedance;

        public float SourceVoltage
        {
            get
            {
                if(IsVoltageSource) return _sourceVoltage;
                throw new ArgumentException("Cannot calculate the voltage of a resistive load directly.");
            }
        }

        protected float _sourceVoltage;

        /// <summary>
        /// The first terminal.
        /// </summary>
        public Node T1;

        /// <summary>
        /// The second terminal.
        /// </summary>
        public Node T2;

        /// <summary>
        /// Get the opposing side node of this component.
        /// </summary>
        /// <param name="start"></param>
        /// <returns></returns>
        public Node GetOpposingSide(Node start)
        {
            if (T1 == start)
            {
                return T2;
            }
            else if (T2 == start)
            {
                return T1;
            }

            throw new Exception("Start node is not a part of this component.");
        }

        public void LinkT1(Node node)
        {
            if (T1 != null)
            {
                T1.ConnectedComponents.Remove(this);
            }

            T1 = node;
            node.ConnectedComponents.Add(this);
        }
        
        public void LinkT2(Node node)
        {
            if (T2 != null)
            {
                T2.ConnectedComponents.Remove(this);
            }

            T2 = node;
            node.ConnectedComponents.Add(this);
        }
    }
}
