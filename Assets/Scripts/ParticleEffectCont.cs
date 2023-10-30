using UnityEngine;

public class ParticleEffectCont : MonoBehaviour
{
    public Transform objectToFollow;
    private ParticleSystem particleSystem;

    private void Start()
    {
        particleSystem = GetComponent<ParticleSystem>();
    }

    private void Update()
    {
        if (objectToFollow != null)
        {
            Vector3 direction = objectToFollow.position - transform.position;

            Vector3 oppositeDirection = -direction;

            particleSystem.transform.rotation = Quaternion.LookRotation(oppositeDirection);

            if (!particleSystem.isPlaying)
            {
                particleSystem.Play();
            }
        }
    }
}
