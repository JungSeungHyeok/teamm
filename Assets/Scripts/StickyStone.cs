using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StickyStone : MonoBehaviour
{
    public enum StoneState { NotFired, Fired, Stopped }
    public StoneState currentState = StoneState.NotFired;

    public float stickyRadius = 50.0f;
    public float stickinessThreshold = 2.0f; // ���ο� ����: ������ �Ӱ谪

    private Rigidbody rb;
    private List<Rigidbody> attachedStones;

    public bool enableStickiness = false;

    //public GameObject particleEffect;
    //private GameObject currentParticle;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        // rb.constraints = RigidbodyConstraints.FreezePositionY; // Y�� �̵� ����

        attachedStones = new List<Rigidbody>();
        StartCoroutine(CheckIfStopped());
    }

    IEnumerator CheckIfStopped()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.5f);

            if (currentState == StoneState.NotFired)
            {
                continue;
            }

            //if (rb.velocity.magnitude < 2f)
            //{
            //    currentState = StoneState.Stopped;
            //    DetachStones(); // 2���� ����ȿ�� ����
            //    break;
            //}
        }
    }

    //private void DetachStones()
    //{
    //    foreach (Rigidbody attachedStone in attachedStones)
    //    {
    //        attachedStone.isKinematic = false;
    //        attachedStone.transform.SetParent(null);
    //    }
    //    attachedStones.Clear();
    //}

    public void FireStone()
    {
        rb.isKinematic = false;
        currentState = StoneState.Fired;
        StartCoroutine(EnableStickinessAfterDelay(0.1f));  // 0.1�� �Ŀ� �����̴� ��� Ȱ��ȭ
    }


    IEnumerator EnableStickinessAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        enableStickiness = true;  // �����̴� ��� Ȱ��ȭ
    }

    public void OnCollisionEnter(Collision collision)
    {
        if (currentState != StoneState.Fired || !enableStickiness || rb.velocity.magnitude < stickinessThreshold)
        {
            //Debug.Log("������ �׽�Ʈ");

            return;
        }

        if (collision.collider.tag == "Stone" || 
            collision.collider.tag == "ShinyStone" ||
            collision.collider.tag == "ExplosiveStone" ||
            collision.collider.tag == "StickyStone")
        {
            

            Rigidbody rbNearby = collision.collider.GetComponent<Rigidbody>();
            if (rbNearby != null && !attachedStones.Contains(rbNearby))
            {
                rbNearby.transform.SetParent(transform);

                // �ڼ���, �Ϲݵ� ����ó�� x
                rbNearby.isKinematic = true;
                rb.isKinematic = true;

                //rbNearby.constraints = RigidbodyConstraints.FreezePositionY; // Y�� �̵� ����
                attachedStones.Add(rbNearby);
            }


            //if (collision.gameObject.CompareTag("Stone") || collision.gameObject.CompareTag("StickyStone") || collision.gameObject.CompareTag("ShinyStone"))
            //{
            //    return;
            //}

            //if (currentParticle == null)
            //{
            //    currentParticle = Instantiate(particleEffect, transform.position, Quaternion.identity);
            //}
            //else
            //{
            //    currentParticle.transform.position = transform.position;
            //    currentParticle.transform.rotation = Quaternion.identity;// ����Ʈ �����̼� ����
            //}
        }
    }
}
