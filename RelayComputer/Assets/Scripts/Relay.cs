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
        [Header("Relay Characteristics")] 
        public float SwitchingDelay = 1f;

        [Header("Components")] 
        public Transform Switch;


        private float _switchTimer;
        private bool _switchEnabled;

        private void Awake()
        {
            _switchTimer = SwitchingDelay;
        }

        private void Update()
        {
            /* Update timer */
            _switchTimer -= Time.deltaTime;
            
            /* If needed, update the switch */
            if (_switchTimer <= 0.0f)
            {
                _switchEnabled = !_switchEnabled;
                _switchTimer = SwitchingDelay;
            }
            
            /* Update graphics */
            if (_switchEnabled)
            {
                Switch.rotation = Quaternion.Euler(0.0f, 0.0f, 30.0f);
            }
            else
            {
                Switch.rotation = Quaternion.Euler(0.0f, 0.0f, -30.0f);
            }
            
        }
    }
}

