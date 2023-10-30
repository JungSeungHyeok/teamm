using System.Collections;
using UnityEngine;

public class MagneticStone : MonoBehaviour
{
    public enum StoneState { NotFired, Fired, Stopped }
    public StoneState currentState = StoneState.NotFired;

    public bool isActive = false; // 프리뷰 스톤에서의 자석기능 비활성화

    public float magneticRadius = 75.0f;
    public float magneticForce = 2500.0f;

    private bool isMagnetActive = false;

    private Rigidbody rb;
    public StoneControler stoneControler;

    public GameObject particleEffect;

    public AudioClip MagSound;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        StartCoroutine(CheckIfStopped());

        stoneControler = FindObjectOfType<StoneControler>();

    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, magneticRadius);
    }

    IEnumerator CheckIfStopped()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.5f);

            if (currentState == StoneState.NotFired) // 프리뷰 상태일때 리턴
            {
                continue;
            }

            if (rb.velocity.magnitude < 3.0f)
            {
                currentState = StoneState.Stopped;
                rb.isKinematic = true;
                isMagnetActive = true;
                GameObject particle = Instantiate(particleEffect, transform.position, Quaternion.identity);

                yield return new WaitForSeconds(4.0f);

                isMagnetActive = false;
                rb.isKinematic = false;
                break;
            }
        }
    }

    public void FireStone()
    {
        currentState = StoneState.Fired;
        StartCoroutine(ActivateMagnetAfterDelay(4.5f));
    }

    IEnumerator ActivateMagnetAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        currentState = StoneState.Stopped;
        rb.isKinematic = true;
        isMagnetActive = true;
        GameObject particle = Instantiate(particleEffect, transform.position, Quaternion.identity);

        yield return new WaitForSeconds(4.0f);

        isMagnetActive = false;
        rb.isKinematic = false;
    }


    void FixedUpdate()
    {
        if (isActive || currentState != StoneState.Stopped)
        {
            return;
        }

        if (!isMagnetActive)
            return;

        Collider[] colliders = Physics.OverlapSphere(transform.position, magneticRadius);

        foreach (Collider nearbyObject in colliders)
        {
            GameObject audioObject = new GameObject("MagSound");
            AudioSource audioSourceOnNewObject = audioObject.AddComponent<AudioSource>();
            audioSourceOnNewObject.clip = MagSound;
            audioSourceOnNewObject.volume = 0.8f;
            audioSourceOnNewObject.Play();
            Destroy(audioObject, MagSound.length);

            Rigidbody rbNearby = nearbyObject.GetComponent<Rigidbody>();
            if (rbNearby != null)
            {
                Vector3 forceDirection = transform.position - rbNearby.transform.position;
                rbNearby.AddForce(forceDirection.normalized * magneticForce * Time.fixedDeltaTime);
            }
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        Rigidbody rb = collision.gameObject.GetComponent<Rigidbody>();
        if (rb != null && isMagnetActive)
        {
            rb.velocity = Vector3.zero;
        }
    }


    //public void ActivateMagnet()
    //{
    //    isActive = true;
    //}

    //public void DeactivateMagnet()
    //{
    //    isActive = false;
    //}

}
