using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapUI : UIBase
{
    public MapController mapController;
    public Camera mapCamera;

    private void Awake()
    {
        //mapController = GetComponent<MapController>();
        mapCamera = GameObject.FindWithTag("MapCamera").GetComponent<Camera>();
    }

    private void OnEnable()
    {
        // ¹Ì´Ï¸Ê ²ô°í Áöµµ ÄÑ±â
    }

    private void OnDisable()
    {
        // Áöµµ ²ô°í ¹Ì´Ï¸Ê ÄÑ±â
    }
}
