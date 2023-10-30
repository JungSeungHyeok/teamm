using UnityEngine;

public class CrashStoneEffect : MonoBehaviour
{
    public GameObject particleEffect;

    public AudioClip crashSound;
    //private AudioSource audioSource;

    private Rigidbody rb;

    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("StickyStone"))
        {
            return;
        }

        // ��Ʈ���� �ϸ� �Ҹ��� �����ż� �߰�
        GameObject audioObject = new GameObject("CrashSound");
        AudioSource audioSourceOnNewObject = audioObject.AddComponent<AudioSource>();
        audioSourceOnNewObject.clip = crashSound;
        audioSourceOnNewObject.Play();

        Destroy(audioObject, crashSound.length);

        GameObject particle = Instantiate(particleEffect, transform.position, Quaternion.identity);

        ParticleSystem ps = particle.GetComponent<ParticleSystem>();
        Destroy(particle, ps.main.duration + ps.main.startLifetime.constantMax);
    }
}
