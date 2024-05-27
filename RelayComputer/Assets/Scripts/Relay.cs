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

        private bool _switchEnabled;

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

