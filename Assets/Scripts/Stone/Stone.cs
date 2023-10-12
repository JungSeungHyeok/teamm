using UnityEngine;

public class Stone : MonoBehaviour
{
    public float friction = 0.2f;  // 마찰 계수
    public float reflectionLoss = 0.1f;  // 반사시 에너지 손실
    private Rigidbody rb;

    //private void Start()
    //{
    //    rb = GetComponent<Rigidbody>();
    //}

    //private void FixedUpdate()
    //{
    //    rb.velocity *= friction;
    //}

    //private void OnCollisionEnter(Collision collision)
    //{
    //    if (collision.gameObject.tag == "Wall")
    //    {
    //        Vector3 velocity = rb.velocity;
    //        velocity = Vector3.Reflect(velocity, collision.contacts[0].normal).normalized * velocity.magnitude * reflectionLoss;
    //        rb.velocity = velocity;
    //    }
    //}

}
