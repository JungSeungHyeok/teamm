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

        // 디스트로이 하면 소리도 삭제돼서 추가
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
