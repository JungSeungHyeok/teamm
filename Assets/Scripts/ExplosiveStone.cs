using UnityEngine;

public class ExplosiveStone : Stone
{
    public float explosionRadius = 5.0f;
    public float explosionForce = 10.0f;

    private float timeSinceSpawn = 0.0f; // 추가된 변수

    void FixedUpdate()
    {
        timeSinceSpawn += Time.deltaTime; // 시간 증가

        if (timeSinceSpawn < 3.0f) // 1초 이내라면 아무것도 하지 않고 반환
        {
            return;
        }

        Rigidbody rb = GetComponent<Rigidbody>();

        if (rb.velocity.magnitude < 0.1f)
        {
            // 주변 오브젝트에 폭발 효과 적용
            Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius);

            foreach (Collider nearbyObject in colliders)
            {
                Rigidbody rbNearby = nearbyObject.GetComponent<Rigidbody>();
                if (rbNearby != null)
                {
                    rbNearby.AddExplosionForce(explosionForce, transform.position, explosionRadius);
                }
            }

            Destroy(gameObject);
        }
    }
}
