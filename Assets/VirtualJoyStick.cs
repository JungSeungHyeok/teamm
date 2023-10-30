using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class VirtualJoyStick : MonoBehaviour, IDragHandler, IEndDragHandler
{
    public enum Axis
    { Horizontal, Vertical,}

    public Image stick;
    public float radius;
    private Vector3 originalPoint;
    private RectTransform rectTr;

    private void Start()
    {
        rectTr = GetComponent<RectTransform>();

        originalPoint = stick.rectTransform.position;
    }

    public float GetAxis(Axis axis)
    {
        return 0f;
    }

    public void OnDrag(PointerEventData eventData)
    {
        Debug.Log(eventData.position);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Vector3 eventWorldPos;

        RectTransformUtility.ScreenPointToWorldPointInRectangle(
            rectTr, eventData.position, null, out eventWorldPos);
    }

    
}
