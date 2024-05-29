using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
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
    
    /* Wiring Variables */
    [Header("Wiring")] 
    public Wire pf_Wire;
    private bool _isWiring;
    private Wire _rootWire;
    private Wire _currentHorizontal;
    private Wire _currentVertical;
    private Vector2 _wireAnchor;

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
                    interactable.OnInteract(GetWorldMousePosition());
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
        
        /* If needed, update wiring */
        if(_isWiring) HandleWiringUpdates();
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

    /// <summary>
    /// Handle the expansion/navigation of a wire.
    /// </summary>
    private void HandleWiringUpdates()
    {
        var worldPos = GetWorldMousePosition();
        
        /* Update initial direction (perp to original direction) */
        if (_rootWire.IsVertical)
        {
            /* horizontal first */
            _currentHorizontal.transform.localScale =
                _currentHorizontal.transform.localScale.WithX(Mathf.Abs(_wireAnchor.x - worldPos.x));
            _currentHorizontal.transform.position = new Vector3((_wireAnchor.x + worldPos.x) / 2.0f, _wireAnchor.y);
            
            /* then vertical */
            _currentVertical.transform.localScale =
                _currentVertical.transform.localScale.WithX(Mathf.Abs(_wireAnchor.y - worldPos.y));
            _currentVertical.transform.position = new Vector3(worldPos.x, (worldPos.y + _wireAnchor.y) / 2.0f);
        }
        else
        {
            /* vertical first */
            _currentVertical.transform.localScale =
                _currentVertical.transform.localScale.WithX(Mathf.Abs(_wireAnchor.y - worldPos.y));
            _currentVertical.transform.position = new Vector3(_wireAnchor.x, (worldPos.y + _wireAnchor.y) / 2.0f);
            
            /* then horizontal */
            _currentHorizontal.transform.localScale =
                _currentHorizontal.transform.localScale.WithX(Mathf.Abs(_wireAnchor.x - worldPos.x));
            _currentHorizontal.transform.position = new Vector3((_wireAnchor.x + worldPos.x) / 2.0f, worldPos.y);
        }
    }

    /// <summary>
    /// Start stringing a wire from a given wire/position.
    /// </summary>
    /// <param name="position"></param>
    /// <param name="source"></param>
    public void WiringUpdate(Vector2 position, Wire wire)
    {
        /* Clamp the position to the grid */
        position *= 2;
        position = new Vector2(Mathf.Round(position.x), Mathf.Round(position.y));
        position /= 2;
        
        if (!_isWiring)
        {
            /* We are making a new wire */
            _isWiring = true;
            _wireAnchor = position;
            _rootWire = wire;
            
            /* Generate a horizontal and vertical wire to use */
            _currentVertical = Instantiate(pf_Wire);
            _currentHorizontal = Instantiate(pf_Wire);
            _currentVertical.name = "WireV";
            _currentHorizontal.name = "WireH";
            _currentVertical.transform.rotation = Quaternion.Euler(0f, 0f, 90f);
            _currentHorizontal.transform.localScale = new Vector3(0f, _currentHorizontal.transform.localScale.y,
                _currentHorizontal.transform.localScale.z);
            _currentVertical.transform.localScale = new Vector3(0f, _currentVertical.transform.localScale.y,
                _currentVertical.transform.localScale.z);
            _currentHorizontal.transform.position = _wireAnchor;
            _currentVertical.transform.position = _wireAnchor;

            _currentVertical.Node = _rootWire.Node;
            _currentHorizontal.Node = _rootWire.Node;

            _currentVertical.gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");
            _currentHorizontal.gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");
        }
        else
        {
            _isWiring = false;
            
            /* update the position/size one last time */
            if (_rootWire.IsVertical)
            {
                /* horizontal first */
                _currentHorizontal.transform.localScale =
                    _currentHorizontal.transform.localScale.WithX(Mathf.Abs(_wireAnchor.x - position.x));
                _currentHorizontal.transform.position = new Vector3((_wireAnchor.x + position.x) / 2.0f, _wireAnchor.y);
            
                /* then vertical */
                _currentVertical.transform.localScale =
                    _currentVertical.transform.localScale.WithX(Mathf.Abs(_wireAnchor.y - position.y));
                _currentVertical.transform.position = new Vector3(position.x, (position.y + _wireAnchor.y) / 2.0f);
            }
            else
            {
                /* vertical first */
                _currentVertical.transform.localScale =
                    _currentVertical.transform.localScale.WithX(Mathf.Abs(_wireAnchor.y - position.y));
                _currentVertical.transform.position = new Vector3(_wireAnchor.x, (position.y + _wireAnchor.y) / 2.0f);
            
                /* then horizontal */
                _currentHorizontal.transform.localScale =
                    _currentHorizontal.transform.localScale.WithX(Mathf.Abs(_wireAnchor.x - position.x));
                _currentHorizontal.transform.position = new Vector3((_wireAnchor.x + position.x) / 2.0f, position.y);
            }
            
            /* Check, if the vertical/horizontal position is zero, we don't need both wires */
            if (_currentHorizontal.transform.localScale.x == 0.0f)
            {
                Destroy(_currentHorizontal.gameObject);
                _currentHorizontal = null;
            }
            else
            {
                _currentHorizontal.Node.Merge(wire.Node);
                wire.Node = _currentHorizontal.Node;
                _currentHorizontal.gameObject.layer = LayerMask.NameToLayer("Default");
            }

            if (_currentVertical.transform.localScale.x == 0.0f)
            {
                Destroy(_currentVertical.gameObject);
                _currentVertical = null;
            }
            else
            {
                _currentVertical.Node.Merge(wire.Node);
                wire.Node = _currentVertical.Node;
                _currentVertical.gameObject.layer = LayerMask.NameToLayer("Default");
            }
        }
    }
}
