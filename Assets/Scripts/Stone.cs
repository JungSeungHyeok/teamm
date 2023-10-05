using UnityEngine;

public class Stone : MonoBehaviour
{
    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Wall")
        {
            Vector3 averageNormal = Vector3.zero;

            foreach (ContactPoint contact in collision.contacts)
            {
                // 각 접촉 지점의 법선 벡터를 더함
                averageNormal += contact.normal;
            }

            // 평균 법선 벡터를 계산
            averageNormal = averageNormal / collision.contacts.Length;

            // 반사 벡터 계산
            Vector3 reflectedVelocity = rb.velocity - 2 * (Vector3.Dot(rb.velocity, averageNormal)) * averageNormal;

            // 힘을 가함
            rb.AddForce(reflectedVelocity - rb.velocity, ForceMode.VelocityChange);
        }
    }
}
