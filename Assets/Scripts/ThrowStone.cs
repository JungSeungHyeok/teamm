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
    private bool isLeftAndRight = true; // 좌우조절
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
            rb.isKinematic = true;  // 물리 효과 비활성화
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

        // 돌 발사 파워 최대, 최소값 제한
        if (currentPower >= maxPower || currentPower <= minPower)
        {
            powerIncrement = -powerIncrement;
        }

        // 현재 힘 = 슬라이더 업데이트
        powerSlider.value = currentPower;

        // 포워드 변경
        currentDirection = Quaternion.Euler(0, transform.eulerAngles.y + currentAngle - 45f, 0) * Vector3.forward;

        // 화살표 회전 (새로운 코드 부분)
        GameObject[] arrows = GameObject.FindGameObjectsWithTag("AngleArrow");
        foreach (GameObject arrow in arrows)
        {
            if (arrow.transform.IsChildOf(transform)) // 플레이어의 자식 오브젝트인 경우에만 처리
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
        // 테스트는 업데이트 따로 픽스드에서는 마우스 씹힘
        if (Input.GetMouseButtonDown(0))
        {
            Throw(currentDirection);
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
        // 새로운 예시 돌 생성
        // 아래 위치에서 변경
        CreatePreviewStone();

        if (previewStone != null)
        {
            Rigidbody rb = previewStone.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.isKinematic = false;  // 물리 효과 활성화
            }

            rb.AddForce(direction * currentPower, ForceMode.Impulse);

            previewStone = null;  // 기존 참조 제거
        }

    }
}