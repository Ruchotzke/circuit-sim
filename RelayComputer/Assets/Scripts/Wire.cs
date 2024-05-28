using System.Collections;
using System.Collections.Generic;
using RelayComputer;
using UnityEngine;

public class Wire : MonoBehaviour, IInteractable
{
    public CircuitNode Node;

    /// <summary>
    /// Is this a vertical or horizontal wire.
    /// </summary>
    public bool IsVertical => transform.rotation.z != 0;

    public void OnInteract(Vector2 position)
    {
        InteractionManager.Instance.WiringUpdate(position, this);
    }
}
