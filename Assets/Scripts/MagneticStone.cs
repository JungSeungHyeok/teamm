using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static StoneControler;

public class MagneticStone : MonoBehaviour
{
    public enum StoneState { NotFired, Fired, Stopped }
    public StoneState currentState = StoneState.NotFired;

    public bool isActive = false; // 프리뷰 스톤에서의 자석기능 비활성화

    public float magneticRadius = 10.0f;
    public float magneticForce = 200.0f;

    private bool isMagnetActive = false;
    public bool isIndicatorActive = false;

    private Rigidbody rb;
    public StoneControler stoneControler;
    public GameObject magneticRadiusIndicator;  // 원 모양의 GameObject
    

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

            if (rb.velocity.magnitude < 2.0f)
            {
                currentState = StoneState.Stopped;
                rb.isKinematic = true;
                isMagnetActive = true;
                isIndicatorActive = true;

                yield return new WaitForSeconds(4.0f);

                isMagnetActive = false;
                rb.isKinematic = false;
                isIndicatorActive = false;
                break;
            }
        }
    }

    public void FireStone()
    {
        currentState = StoneState.Fired; // 쓰로우함수 호출되면 발사된 상태로 변경
    }

    private void Update()
    {
        if (isIndicatorActive)
        {
            magneticRadiusIndicator.SetActive(true);
            magneticRadiusIndicator.transform.localScale = new Vector3(magneticRadius, 1, magneticRadius);
        }
        else
        {
            magneticRadiusIndicator.SetActive(false);
        }
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


    public void ActivateMagnet()
    {
        isActive = true;
    }

    public void DeactivateMagnet()
    {
        isActive = false;
    }

}
