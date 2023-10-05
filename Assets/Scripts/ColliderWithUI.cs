using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
[RequireComponent(typeof(RectTransform))]
public class SyncColliderWithUI : MonoBehaviour
{
    private BoxCollider boxCollider;
    private RectTransform rectTransform;

    void Start()
    {
        boxCollider = GetComponent<BoxCollider>();
        rectTransform = GetComponent<RectTransform>();
    }

    void Update()
    {
        // 스케일을 고려하여 BoxCollider의 크기 설정
        Vector3 newScale = rectTransform.localScale;
        boxCollider.size = new Vector3(rectTransform.rect.width * newScale.x, rectTransform.rect.height * newScale.y, boxCollider.size.z);
    }
}
