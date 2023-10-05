using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;


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
    public GameObject stonePrefab;
    public float throwSpeed = 10f;

    // 추후 볼타입 변경 생각해서 리스트로변경 // 이미지x
    public List<GameObject> stonePrefabs;
    public GameObject currentStonePrefab;  // 현재 돌 프리팹
    private int currentStoneIndex = 0;

    public Slider powerSlider;
    public float minPower = 10f;
    public float maxPower = 20f;

    public Image projectileArrow;

    // 각도 및 파워 증가량 조절
    public float angleIncrement = 20f;
    public float powerIncrement = 20f;

    private float currentPower;

    private Vector3 resetAngle;
    private float currentAngle = 0f;

    private Vector3 currentDirection;

    private bool isLeftndRight = true; // 좌우조절

    private GameObject previewStone;



    private void Start()
    {
        // 슬라이더 초기 설정
        powerSlider.minValue = minPower;
        powerSlider.maxValue = maxPower;
        currentPower = minPower;
        powerSlider.value = currentPower;

        //유니티에서 수정한 화살표의 각도
        resetAngle = projectileArrow.transform.eulerAngles;

        // 초기 돌 프맆
        if (stonePrefabs.Count > 0)
        {
            currentStonePrefab = stonePrefabs[currentStoneIndex];
        }
        else
        {
            Debug.LogWarning("stonePrefabs 리스트가 비어 있음");
        }

        CreatePreviewStone();
        // 체인지투타입에서 구현하기 전에 테스트
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
        projectileArrow.rectTransform.localEulerAngles = resetAngle + new Vector3(0, transform.eulerAngles.y + currentAngle - 45f, 0);
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
        // 새로운 예시 돌 생성
        // 아래 위치에서 변경
        CreatePreviewStone();


        if (previewStone != null)
        {
            Rigidbody rb = previewStone.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.isKinematic = false;  // 물리 효과 활성화
                // rb.AddForce(direction * currentPower, ForceMode.Impulse); // 물리적 힘 적용
            }

            // 반사 적용
            PhysicMaterial physicMat = new PhysicMaterial();
            physicMat.bounciness = 1;
            previewStone.GetComponent<Collider>().material = physicMat;

            // 힘 다시 추가
            rb.AddForce(direction * currentPower, ForceMode.Impulse); // 현재 파워를 날림

            previewStone = null;  // 기존 참조 제거
        }



    }
}
