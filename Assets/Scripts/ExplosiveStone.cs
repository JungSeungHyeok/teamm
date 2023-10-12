using System.Collections;
using UnityEngine;

public class ExplosiveStone : MonoBehaviour
{
    public enum StoneState { NotFired, Fired, Stopped }
    public StoneState currentState = StoneState.NotFired;

    public float explosionRadius = 50.0f;
    public float explosionForce = 0f;

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        StartCoroutine(CheckIfStopped());
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }

    IEnumerator CheckIfStopped()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.5f);

            if (currentState == StoneState.NotFired) // 프리뷰 상태일 때 리턴 매그니틱과 동일
            {
                continue;
            }

            if (rb.velocity.magnitude < 2.0f)
            {
                currentState = StoneState.Stopped;

                // 주변 오브젝트에 폭발 효과 적용
                Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius);
                foreach (Collider nearbyObject in colliders)
                {
                    Rigidbody rbNearby = nearbyObject.GetComponent<Rigidbody>();
                    if (rbNearby != null)
                    {
                        rbNearby.AddExplosionForce(explosionForce, transform.position, explosionRadius);
                    }

                    if (nearbyObject.tag == "Terrain")
                    {
                        Destroy(nearbyObject.gameObject); // 지형지물 제거
                    }
                }

                Destroy(gameObject);
                break;
            }
        }
    }

    public void FireStone()
    {
        currentState = StoneState.Fired; // 발사된 상태로 변경
    }

    void FixedUpdate()
    {
        if (currentState != StoneState.Stopped) // 발사된 상태가 아니면 리턴
        {
            return;
        }

        // 여기서 필요한 코드를 추가할 수 있습니다.
    }
}
