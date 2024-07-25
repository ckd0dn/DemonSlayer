using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class MapController : MonoBehaviour
{
    private Player player;
    private Camera mapCamera;   
    private Vector3 originalCameraPosition;
    private Vector3 lastMousePosition;
    private bool isDragging;
    public float baseDragSpeed = 0.1f;
    public float zoomSpeed = 0.1f;
    public float minZoom = 30f;
    public float maxZoom = 80f;    

    public event Action<Vector2> OnDrag;
    public event Action<float> OnZoom;

    private void Awake()
    {
        player = GameManager.Instance.Player;
        mapCamera = GameManager.Instance.mapCamera;
    }

    private void Start()
    {        
        player.Input.UIActions.Drag.performed += context => OnDrag?.Invoke(context.ReadValue<Vector2>());
        player.Input.UIActions.Zoom.performed += context => OnZoom?.Invoke(context.ReadValue<Vector2>().y);
    }

    private void OnEnable()
    {
        OnDrag += HandleDrag;
        OnZoom += HandleZoom;
        mapCamera.orthographicSize = maxZoom;
        originalCameraPosition = mapCamera.transform.position;
    }

    private void OnDisable()
    {
        OnDrag -= HandleDrag;
        OnZoom -= HandleZoom;
        mapCamera.orthographicSize = minZoom;
    }

    private void HandleDrag(Vector2 mousePosition)
    {
        if (UIManager.Instance.mainMenuUI.currentTapIndex == 3)
        {
            if (Mouse.current.leftButton.isPressed)
            {
                if (!isDragging)
                {
                    isDragging = true;
                    lastMousePosition = mousePosition;
                }
                else
                {
                    Vector3 delta = mousePosition - (Vector2)lastMousePosition;
                    float dragSpeed = baseDragSpeed * (mapCamera.orthographicSize / maxZoom);
                    mapCamera.transform.position -= new Vector3(delta.x, delta.y, 0) * dragSpeed;
                    lastMousePosition = mousePosition;
                }
            }
            else
            {
                isDragging = false;
            }
        }
    }

    private void HandleZoom(float scrollData)
    {
        if (UIManager.Instance.mainMenuUI.currentTapIndex == 3)
        {
            mapCamera.orthographicSize = Mathf.Clamp(mapCamera.orthographicSize - scrollData * zoomSpeed, minZoom, maxZoom);
        }
    }

    public void ResetCameraPosition()
    {
        mapCamera.transform.position = originalCameraPosition;
    }
}
