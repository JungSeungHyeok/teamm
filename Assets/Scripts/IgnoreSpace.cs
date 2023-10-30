using UnityEngine.EventSystems;
using UnityEngine;

public class IgnoreSpace : MonoBehaviour, IUpdateSelectedHandler
{
    public void OnUpdateSelected(BaseEventData eventData)
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            eventData.Use();
        }
    }
}
