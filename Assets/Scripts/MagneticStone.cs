using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagneticStone : MonoBehaviour
{
    public float magneticRadius = 10.0f; // 자석 범위
    public float magneticForce = 200.0f; // 자석 힘

    void FixedUpdate()
    {
        // 자석 범위 내의 모든 오브젝트를 찾음
        Collider[] colliders = Physics.OverlapSphere(transform.position, magneticRadius);

        foreach (Collider nearbyObject in colliders)
        {
            Rigidbody rbNearby = nearbyObject.GetComponent<Rigidbody>();
            if (rbNearby != null)
            {
                // 자석 효과 적용 (오브젝트를 자신에게 당김)
                Vector3 forceDirection = transform.position - rbNearby.transform.position;
                rbNearby.AddForce(forceDirection.normalized * magneticForce * Time.fixedDeltaTime);
            }
        }
    }
}
