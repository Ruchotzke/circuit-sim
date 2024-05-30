using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RelayComputer
{
    /// <summary>
    /// A relay model.
    /// </summary>
    public class Relay : MonoBehaviour
    {
        [Header("Components")] 
        public Transform Switch;

        public CircuitPin CoilAPin;
        public CircuitPin CoilBPin;
        public CircuitPin StemPin;
        public CircuitPin NOPin;
        public CircuitPin NCPin;

        private bool _switchEnabled;

        private void Awake()
        {
            /* generate pins */
            CoilAPin.ParentComponent = gameObject;
            CoilAPin.Node = new CircuitNode(false, false);
            CoilAPin.Node.Relays.Add(this);
            CoilBPin.ParentComponent = gameObject;
            CoilBPin.Node = new CircuitNode(false, false);
            CoilBPin.Node.Relays.Add(this);
            StemPin.ParentComponent = gameObject;
            StemPin.Node = new CircuitNode(false, false);
            StemPin.Node.Relays.Add(this);
            NOPin.ParentComponent = gameObject;
            NOPin.Node = new CircuitNode(false, false);
            NOPin.Node.Relays.Add(this);
            NCPin.ParentComponent = gameObject;
            NCPin.Node = new CircuitNode(false, false);
            NCPin.Node.Relays.Add(this);
        }

        void UpdateGraphic()
        {
            if (_switchEnabled)
            {
                Switch.rotation = Quaternion.Euler(0.0f, 0.0f, -50.0f);
            }
            else
            {
                Switch.rotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);
            }
        }

        /// <summary>
        /// Convert nodes during a merge.
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        public void RelinkNode(CircuitNode from, CircuitNode to)
        {
            if (CoilAPin.Node == from) CoilAPin.Node = to;
            if (CoilBPin.Node == from) CoilBPin.Node = to;
            if (StemPin.Node == from) StemPin.Node = to;
            if (NOPin.Node == from) NOPin.Node = to;
            if (NCPin.Node == from) NCPin.Node = to;
        }
        
        /// <summary>
        /// Get the opposing side of this component's coil.
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public CircuitNode GetOpposingCoil(CircuitNode source)
        {
            /* Coil */
            if (CoilAPin.Node != CoilBPin.Node)
            {
                if (CoilAPin.Node == source) return CoilBPin.Node;
                if (CoilBPin.Node == source) return CoilAPin.Node;
            }

            return null;
        }

        /// <summary>
        /// Get the opposing side of this component's coil.
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public CircuitNode GetOpposingSwitch(CircuitNode source)
        {
            /* Switch Status */
            bool switchedOn = false;
            if (CoilAPin.Node.Status == CircuitNode.NodeStatus.SOLVED &&
                CoilBPin.Node.Status == CircuitNode.NodeStatus.SOLVED)
            {
                var diff = Mathf.Abs(CoilAPin.Node.LastVoltage - CoilBPin.Node.LastVoltage);
                if (diff > 9.0f)
                {
                    switchedOn = true;
                }
            }
            
            /* Check the correct pins */
            if (switchedOn)
            {
                if (StemPin.Node == NOPin.Node) return null;
                if (StemPin.Node == source) return NOPin.Node;
                if (NOPin.Node == source) return StemPin.Node;
            }
            else
            {
                if (StemPin.Node == NCPin.Node) return null;
                if (StemPin.Node == source) return NCPin.Node;
                if (NCPin.Node == source) return StemPin.Node;
            }

            return null;
        }
    }
}

