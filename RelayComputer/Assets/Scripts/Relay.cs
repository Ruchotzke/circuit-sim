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

        public CircuitNode CoilA;
        public CircuitNode CoilB;
        public CircuitNode Stem;
        public CircuitNode NO;
        public CircuitNode NC;

        private bool _switchEnabled;

        private void Awake()
        {
            /* generate pins */
            CoilA = new CircuitNode(false, false);
            CoilB = new CircuitNode(false, false);
            Stem = new CircuitNode(false, false);
            NO = new CircuitNode(false, false);
            NC = new CircuitNode(false, false);
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

