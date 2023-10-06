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


public class StoneControler : MonoBehaviour
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
    private float currentAngle;
    private Vector3 currentDirection;
    private bool isLeftAndRight = true; // �¿�����
    private Vector3 resetAngle;
    private GameObject previewStone;
    public string arrowTag = "AngleArrow";

    private bool isLeftndRight = true;

    private enum ThrowState { Aiming, Charging, ReadyToThrow }
    private ThrowState currentState = ThrowState.Aiming;

    private int weight = 2;


    private void Start()
    {
        powerSlider.minValue = minPower;
        powerSlider.maxValue = maxPower;
        currentPower = minPower;
        powerSlider.value = currentPower;

        resetAngle = projectileArrow.transform.eulerAngles;

        CreatePreviewStone();

    }

    public void FixedUpdate()
    {

        switch (currentState)
        {
            case ThrowState.Aiming:
                UpdateAngle();
                break;

            case ThrowState.Charging:
                UpdatePower();
                break;
        }
    }

    private void Update()
    {
        switch (currentState)
        {
            case ThrowState.Aiming:
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    currentState = ThrowState.Charging;
                }
                break;

            case ThrowState.Charging:
                if (Input.GetKeyUp(KeyCode.Space))
                {
                    Throw(currentDirection);
                    currentState = ThrowState.Aiming;
                    CreatePreviewStone(); // �߻� �Ŀ��� ���ο� previewStone�� ����
                }
                break;
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


        // ���� ���� �� �ȵ�
        // direction = Quaternion.Euler(0, -45, 0) * direction;

        // ���ο� ���� �� ����
        // �Ʒ� ��ġ���� ����
        CreatePreviewStone();

        if (previewStone != null)
        {
            Rigidbody rb = previewStone.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.isKinematic = false;  // ���� ȿ�� Ȱ��ȭ
                rb.AddForce(direction * currentPower, ForceMode.Impulse);
            }

            

            previewStone = null;  // ���� ���� ����
        }

    }



    private void UpdateAngle()
    {
        currentAngle += (isLeftndRight ? 1 : -1) * angleIncrement * Time.deltaTime;

        if (currentAngle >= 90f || currentAngle <= 0f)
        {
            isLeftndRight = !isLeftndRight;
        }

        currentDirection = Quaternion.Euler(0, currentAngle, 0) * Vector3.forward;

        //currentDirection = Quaternion.Euler(0, transform.eulerAngles.y + currentAngle - 45f, 0) * Vector3.forward;

        GameObject[] arrows = GameObject.FindGameObjectsWithTag("AngleArrow");
        foreach (GameObject arrow in arrows)
        {
            if (arrow.transform.IsChildOf(transform)) // �÷��̾��� �ڽ� ������Ʈ�� ��쿡�� ó��
            {
                RectTransform arrowRect = arrow.GetComponent<RectTransform>();
                if (arrowRect != null)
                {
                    arrowRect.localEulerAngles = resetAngle + new Vector3(0, currentAngle - 45, 0);
                }
            }
        }
    }

    private void UpdatePower()
    {
        currentPower += powerIncrement * Time.deltaTime * weight;

        if (currentPower >= maxPower || currentPower <= minPower)
        {
            powerIncrement = -powerIncrement;
        }

        powerSlider.value = currentPower;
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






}