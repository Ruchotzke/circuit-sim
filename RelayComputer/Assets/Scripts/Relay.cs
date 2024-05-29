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
    }
}

