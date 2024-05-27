using System.Collections;
using System.Collections.Generic;
using RelayComputer;
using UnityEngine;

public class InteractionManager : MonoBehaviour
{
    /* Singleton */
    /// <summary>
    /// The singleton reference for the interaction manager.
    /// </summary>
    public static InteractionManager Instance;
    
    /* Camera reference */
    private Camera _camera;

    /* Dragging Variables */
    private Vector3 _dragOrigin;
    private Vector3 _difference;
    private bool _dragging = false;
    
    /* Zooming Variables */
    [Header("Zoom Options")] 
    public float MaxZoom;
    public float MinZoom;
    public float ZoomSpeed;
    
    void Awake(){
        /* Configure Singleton */
        if (Instance == null)
        {
            Instance = this;
            _camera = Camera.main;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        /* movement */
        HandleCameraMotion();
        
        /* input */
        HandleInput();
    }

    /// <summary>
    /// Handle input/interaction.
    /// </summary>
    private void HandleInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            var found = GetColliderUnderMouse();
            if (found != null)
            {
                if (found.TryGetComponent(out IInteractable interactable))
                {
                    interactable.OnInteract();
                }
            }
        }
    }

    /// <summary>
    /// Handle any camera movement/zoom items.
    /// </summary>
    private void HandleCameraMotion()
    {
        /* Handle Camera Dragging */
        if (Input.GetMouseButton(2))
        {
            /* Mouse is down - we should be dragging */
            _difference = (Vector3)GetWorldMousePosition() - _camera.transform.position;
            if (!_dragging)
            {
                /* We are starting a drag */
                _dragging = true;
                _dragOrigin = (Vector3)GetWorldMousePosition();
            }
        }
        else
        {
            /* Mouse is up, we're not dragging */
            _dragging = false;
        }
        
        /* If we're dragging, move the camera */
        if (_dragging)
        {
            _camera.transform.position = _dragOrigin - _difference;
        }
        
        /* Detect the change in mouse wheel and zoom */
        HandleZoom();
    }
    
    /// <summary>
    /// Get the world space coordinates of the mouse.
    /// </summary>
    /// <returns></returns>
    public Vector2 GetWorldMousePosition()
    {
        return _camera.ScreenToWorldPoint(Input.mousePosition);
    }

    /// <summary>
    /// Return a gameobject under the mouse if applicable.
    /// </summary>
    /// <returns></returns>
    public GameObject GetColliderUnderMouse()
    {
        var worldPos = GetWorldMousePosition();
        RaycastHit2D hit = Physics2D.Raycast(worldPos, Vector2.zero, 0);
        return hit.collider != null ? hit.transform.gameObject : null;
    }

    /// <summary>
    /// Handle Zooming
    /// </summary>
    private void HandleZoom()
    {
        /* Get the scroll wheel change */
        var scroll = Input.GetAxis("Mouse ScrollWheel");
        
        /* Move the zoom level of the camera */
        float diff = -scroll * ZoomSpeed;
        _camera.orthographicSize = Mathf.Clamp(_camera.orthographicSize + diff, MaxZoom, MinZoom);
    }
}
