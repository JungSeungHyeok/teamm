using UnityEngine;
using System.Collections;

public class ColliderController : MonoBehaviour
{
    private Collider collider;
    private float colliderSize;
    private Vector3 targetPosition;

    private void Start()
    {
        collider = GetComponent<Collider>();
        colliderSize = collider.bounds.size.x;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Vector3 playerPosition = other.transform.position;
            targetPosition = playerPosition; // 목표 위치 초기화

            if (playerPosition.x > transform.position.x) // 왼쪽 벽
            {
                float newX = transform.position.x + colliderSize;
                targetPosition = new Vector3(newX, playerPosition.y, playerPosition.z);
            }
            else if (playerPosition.x < transform.position.x) // 오른쪽 벽
            {
                float newX = transform.position.x - colliderSize;
                targetPosition = new Vector3(newX, playerPosition.y, playerPosition.z);
            }

            StartCoroutine(SmoothMove(other.transform, targetPosition));
        }
    }

    private IEnumerator SmoothMove(Transform transformToMove, Vector3 target)
    {
        float timeToMove = 0.2f;
        float t = 0;

        Vector3 start = transformToMove.position;

        while (t < 1)
        {
            t += Time.deltaTime / timeToMove;
            transformToMove.position = Vector3.Lerp(start, target, t);
            yield return null;
        }
    }
}
