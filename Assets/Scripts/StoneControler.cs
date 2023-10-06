using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.SceneManagement;


//// 나중에 쪼인트 생각하기
//// 나중에 쪼인트 생각하기

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
    private bool isLeftAndRight = true; // 좌우조절
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
                    CreatePreviewStone(); // 발사 후에만 새로운 previewStone을 생성
                }
                break;
        }

        // 돌 타입 변경 테스트
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


        // 각도 보정 잘 안됨
        // direction = Quaternion.Euler(0, -45, 0) * direction;

        // 새로운 예시 돌 생성
        // 아래 위치에서 변경
        CreatePreviewStone();

        if (previewStone != null)
        {
            Rigidbody rb = previewStone.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.isKinematic = false;  // 물리 효과 활성화
                rb.AddForce(direction * currentPower, ForceMode.Impulse);
            }

            

            previewStone = null;  // 기존 참조 제거
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
            if (arrow.transform.IsChildOf(transform)) // 플레이어의 자식 오브젝트인 경우에만 처리
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
            rb.isKinematic = true;  // 물리 효과 비활성화
        }
    }






}