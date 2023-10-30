using UnityEngine;

public class StickyCrashStoneEffect : MonoBehaviour
{
    public GameObject particleEffect;

    public AudioClip StickyCrashSound;

    private Rigidbody rb;

    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Stone") 
            || collision.gameObject.CompareTag("ShinyStone")
            || collision.gameObject.CompareTag("StickyStone"))
        {
            return;
        }

        // ��Ʈ���� �ϸ� �Ҹ��� �����ż� �߰�
        GameObject audioObject = new GameObject("StickyCrashSound");
        AudioSource audioSourceOnNewObject = audioObject.AddComponent<AudioSource>();
        audioSourceOnNewObject.clip = StickyCrashSound;
        audioSourceOnNewObject.Play();

        Destroy(audioObject, StickyCrashSound.length);

        GameObject particle = Instantiate(particleEffect, transform.position, Quaternion.identity);

        ParticleSystem ps = particle.GetComponent<ParticleSystem>();
        Destroy(particle, ps.main.duration + ps.main.startLifetime.constantMax);
    }
}
