using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.SceneManagement;


//// ���߿� ����Ʈ �����ϱ�
//// ���߿� ����Ʈ �����ϱ�

//[System.Serializable]
//public class StoneInfo
//{
//    public GameObject prefab;
//    public Material material;
//}


public class ThrowStone : MonoBehaviour
{
    public StoneManager stoneManager;
    public float throwSpeed = 10f;

    public Slider powerSlider;
    public float minPower = 10f;
    public float maxPower = 20f;

    public Image projectileArrow;
    public float angleIncrement = 20f;
    public float powerIncrement = 20f;

    private float currentPower;
    private float currentAngle = 0f;
    private Vector3 currentDirection;
    private bool isLeftAndRight = true; // �¿�����
    private Vector3 resetAngle;
    private GameObject previewStone;
    public string arrowTag = "AngleArrow";

    private bool isLeftndRight = true;

    private void Start()
    {
        powerSlider.minValue = minPower;
        powerSlider.maxValue = maxPower;
        currentPower = minPower;
        powerSlider.value = currentPower;

        resetAngle = projectileArrow.transform.eulerAngles;

        CreatePreviewStone();
    }


    private void CreatePreviewStone()
    {
        if (previewStone != null)
        {
            Destroy(previewStone);
        }

        previewStone = stoneManager.CreateStone(transform.position, Quaternion.identity);
        Rigidbody rb = previewStone.GetComponent<Rigidbody>();

        if (rb != null)
        {
            rb.isKinematic = true;  // ���� ȿ�� ��Ȱ��ȭ
        }
    }


    public void FixedUpdate()
    {
        currentAngle += (isLeftndRight ? 1 : -1) * angleIncrement * Time.deltaTime;

        if (currentAngle >= 90f || currentAngle <= 0f)
        {
            isLeftndRight = !isLeftndRight;
        }

        currentPower += powerIncrement * Time.deltaTime;

        // �� �߻� �Ŀ� �ִ�, �ּҰ� ����
        if (currentPower >= maxPower || currentPower <= minPower)
        {
            powerIncrement = -powerIncrement;
        }

        // ���� �� = �����̴� ������Ʈ
        powerSlider.value = currentPower;

        // ������ ����
        currentDirection = Quaternion.Euler(0, transform.eulerAngles.y + currentAngle - 45f, 0) * Vector3.forward;

        // ȭ��ǥ ȸ�� (���ο� �ڵ� �κ�)
        GameObject[] arrows = GameObject.FindGameObjectsWithTag("AngleArrow");
        foreach (GameObject arrow in arrows)
        {
            if (arrow.transform.IsChildOf(transform)) // �÷��̾��� �ڽ� ������Ʈ�� ��쿡�� ó��
            {
                RectTransform arrowRect = arrow.GetComponent<RectTransform>();
                if (arrowRect != null)
                {
                    arrowRect.localEulerAngles = resetAngle + new Vector3(0, transform.eulerAngles.y + currentAngle - 45f, 0);
                }
            }
        }
    }


    private void Update()
    {
        // �׽�Ʈ�� ������Ʈ ���� �Ƚ��忡���� ���콺 ����
        if (Input.GetMouseButtonDown(0))
        {
            Throw(currentDirection);
        }

        // �� Ÿ�� ���� �׽�Ʈ
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            stoneManager.ChangeStoneToType(0);
            CreatePreviewStone();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            stoneManager.ChangeStoneToType(1);
            CreatePreviewStone();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            stoneManager.ChangeStoneToType(2);
            CreatePreviewStone();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            stoneManager.ChangeStoneToType(3);
            CreatePreviewStone();
        }
    }

    

    public void Throw(Vector3 direction)
    {
        // ���ο� ���� �� ����
        // �Ʒ� ��ġ���� ����
        CreatePreviewStone();

        if (previewStone != null)
        {
            Rigidbody rb = previewStone.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.isKinematic = false;  // ���� ȿ�� Ȱ��ȭ
            }

            rb.AddForce(direction * currentPower, ForceMode.Impulse);

            previewStone = null;  // ���� ���� ����
        }

    }
}