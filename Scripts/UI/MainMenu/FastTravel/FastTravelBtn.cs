using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class FastTravelBtn : MonoBehaviour, IPointerEnterHandler, ISelectHandler
{
    public FastTravelUI fastTravelUI;
    public Vector3 checkpointPosition;

    public void Init(FastTravelUI ui, Vector3 position)
    {
        fastTravelUI = ui;
        checkpointPosition = position;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        fastTravelUI.OnCheckpointButton(checkpointPosition);
    }

    public void OnSelect(BaseEventData eventData)
    {
        fastTravelUI.OnCheckpointButton(checkpointPosition);
    }
}
