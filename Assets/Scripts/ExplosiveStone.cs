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

            if (currentState == StoneState.NotFired) // ������ ������ �� ���� �ű״�ƽ�� ����
            {
                continue;
            }

            if (rb.velocity.magnitude < 2.0f)
            {
                currentState = StoneState.Stopped;

                // �ֺ� ������Ʈ�� ���� ȿ�� ����
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
                        Destroy(nearbyObject.gameObject); // �������� ����
                    }
                }

                Destroy(gameObject);
                break;
            }
        }
    }

    public void FireStone()
    {
        currentState = StoneState.Fired; // �߻�� ���·� ����
    }

    void FixedUpdate()
    {
        if (currentState != StoneState.Stopped) // �߻�� ���°� �ƴϸ� ����
        {
            return;
        }

        // ���⼭ �ʿ��� �ڵ带 �߰��� �� �ֽ��ϴ�.
    }
}
