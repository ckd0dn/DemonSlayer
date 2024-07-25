using UnityEngine;
using UnityEngine.EventSystems;

public class Slot : MonoBehaviour
{
    GameObject Icon()
    {
        if(transform.childCount > 0)
            return transform.GetChild(0).gameObject;
        else return null;
    }

    public void OnDrop(PointerEventData eventData)
    {
        if(Icon() == null)
        {
            IconDrag.DraggedIcon.transform.SetParent(transform);
            IconDrag.DraggedIcon.transform.position = transform.position;
        }
    }
}