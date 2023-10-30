using System.Collections;
using UnityEngine;

public class ExplosiveStone : MonoBehaviour
{
    public enum StoneState { NotFired, Fired, Stopped }
    public StoneState currentState = StoneState.NotFired;

    public float explosionRadius = 65.0f;
    public float explosionForce = 5000f;

    public AudioClip explosionSound;
    private AudioSource audioSource;

    private Rigidbody rb;

    private bool isExplosionScheduled = false;

    public float minMetallicStrength = 0.0f;
    public float maxMetallicStrength = 1.0f;

    public Material stoneMaterial;
    private Color originalColor;

    public GameObject particleEffect;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        //StartCoroutine(CheckIfStopped()); // 폭발 조건 수정

        Renderer renderer = GetComponent<Renderer>();
        stoneMaterial = renderer.material;

        originalColor = stoneMaterial.GetColor("_ColorTint");

        audioSource = gameObject.AddComponent<AudioSource>(); 
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }

    //IEnumerator CheckIfStopped() //// 무조건 발사후 5초 폭발로 피드백 반영
    //{
    //    while (true)
    //    {
    //        yield return new WaitForSeconds(0.5f);

    //        if (rb.velocity.magnitude < 2.0f && currentState == StoneState.Fired)
    //        {
    //            currentState = StoneState.Stopped;

    //            Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius);
    //            foreach (Collider nearbyObject in colliders)
    //            {
    //                Rigidbody rbNearby = nearbyObject.GetComponent<Rigidbody>();
    //                if (rbNearby != null)
    //                {
    //                    rbNearby.AddExplosionForce(explosionForce, transform.position, explosionRadius);
    //                }

    //                if (nearbyObject.tag == "Terrain")
    //                {
    //                    Destroy(nearbyObject.gameObject); // 지형지물 제거
    //                }
    //            }
    //            isExplosionScheduled = true;
    //            Destroy(gameObject);
    //            break;
    //        }
    //    }
    //}

    IEnumerator ScheduleExplosion()
    {
        float elapsedTime = 0f;
        float explosionDuration = 4.0f; // 폭발 대기 시간
        float lerpingSpeed = 0.17f;

        while (elapsedTime < explosionDuration)
        {
            float t = elapsedTime / explosionDuration;
            float lerpSpeed = Mathf.Pow(t, lerpingSpeed);
            float metallicStrength = Mathf.Lerp(minMetallicStrength, maxMetallicStrength, t);
            stoneMaterial.SetFloat("_MetallicStrength", metallicStrength);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        yield return new WaitForSeconds(4.0f);

        StartCoroutine(ResetMaterial());
        
        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius);
        foreach (Collider nearbyObject in colliders)
        {
            Rigidbody rbNearby = nearbyObject.GetComponent<Rigidbody>();
            if (rbNearby != null)
            {
                rbNearby.AddExplosionForce(explosionForce, transform.position, explosionRadius);
            }

            GameObject audioObject = new GameObject("ExplosionSound");
            AudioSource audioSourceOnNewObject = audioObject.AddComponent<AudioSource>();
            audioSourceOnNewObject.clip = explosionSound;

            audioSourceOnNewObject.volume = 0.8f;
            audioSourceOnNewObject.Play();
            Destroy(audioObject, explosionSound.length);

            GameObject particle = Instantiate(particleEffect, transform.position, Quaternion.identity);

            if (nearbyObject.tag == "Terrain")
            {
                Destroy(nearbyObject.gameObject); // 지형지물 제거
            }
        }

        Destroy(gameObject);
    }

    IEnumerator ResetMaterial()
    {
        yield return new WaitForSeconds(1.0f);

        stoneMaterial.SetColor("_ColorTint", originalColor);

        stoneMaterial.SetFloat("_MetallicStrength", minMetallicStrength);
    }

    public void FireStone()
    {
        currentState = StoneState.Fired; // 발사 상태로 변경

        // 발사된 후 폭발 예약
        if (!isExplosionScheduled)
        {
            isExplosionScheduled = true;
            StartCoroutine(ScheduleExplosion());
        }
    }
    void FixedUpdate()
    {
        if (currentState != StoneState.Stopped)
        {
            return;
        }
    }
}
