using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;


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
    public GameObject stonePrefab;
    public float throwSpeed = 10f;

    // ���� ��Ÿ�� ���� �����ؼ� ����Ʈ�κ��� // �̹���x
    public List<GameObject> stonePrefabs;
    public GameObject currentStonePrefab;  // ���� �� ������
    private int currentStoneIndex = 0;

    public Slider powerSlider;
    public float minPower = 10f;
    public float maxPower = 20f;

    public Image projectileArrow;

    // ���� �� �Ŀ� ������ ����
    public float angleIncrement = 20f;
    public float powerIncrement = 20f;

    private float currentPower;

    private Vector3 resetAngle;
    private float currentAngle = 0f;

    private Vector3 currentDirection;

    private bool isLeftndRight = true; // �¿�����

    private GameObject previewStone;



    private void Start()
    {
        // �����̴� �ʱ� ����
        powerSlider.minValue = minPower;
        powerSlider.maxValue = maxPower;
        currentPower = minPower;
        powerSlider.value = currentPower;

        //����Ƽ���� ������ ȭ��ǥ�� ����
        resetAngle = projectileArrow.transform.eulerAngles;

        // �ʱ� �� ����
        if (stonePrefabs.Count > 0)
        {
            currentStonePrefab = stonePrefabs[currentStoneIndex];
        }
        else
        {
            Debug.LogWarning("stonePrefabs ����Ʈ�� ��� ����");
        }

        CreatePreviewStone();
        // ü������Ÿ�Կ��� �����ϱ� ���� �׽�Ʈ
    }


    private void CreatePreviewStone()
    {
        if (previewStone != null)
        {
            Destroy(previewStone);
        }

        previewStone = Instantiate(currentStonePrefab, transform.position, Quaternion.identity);
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
        projectileArrow.rectTransform.localEulerAngles = resetAngle + new Vector3(0, transform.eulerAngles.y + currentAngle - 45f, 0);
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
            ChangeStoneToType(0);
            CreatePreviewStone();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            ChangeStoneToType(1);
            CreatePreviewStone();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            ChangeStoneToType(2);
            CreatePreviewStone();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            ChangeStoneToType(3);
            CreatePreviewStone();
        }
    }

    private void ChangeStoneToType(int index)
    {
        if (index < 0 || index >= stonePrefabs.Count) return;

        currentStoneIndex = index;
        currentStonePrefab = stonePrefabs[currentStoneIndex];
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
                // rb.AddForce(direction * currentPower, ForceMode.Impulse); // ������ �� ����
            }

            // �ݻ� ����
            PhysicMaterial physicMat = new PhysicMaterial();
            physicMat.bounciness = 1;
            previewStone.GetComponent<Collider>().material = physicMat;

            // �� �ٽ� �߰�
            rb.AddForce(direction * currentPower, ForceMode.Impulse); // ���� �Ŀ��� ����

            previewStone = null;  // ���� ���� ����
        }



    }
}
