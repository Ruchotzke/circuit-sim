using System;
using UnityEngine;

namespace RelayComputer
{
    /// <summary>
    /// Any interactable monobehaviour component.
    /// </summary>
    public interface IInteractable
    {
        /// <summary>
        /// An interaction event.
        /// </summary>
        public void OnInteract(Vector2 position);
    }
}