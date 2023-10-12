using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class StoneControler : MonoBehaviour
{

    SceneManager sceneManager;

    private int currentBallCount = 0;
    public TextMeshProUGUI ballCountText;

    public PlayerMovement playerMovement; // 플레이어 움직임 제어

    public ScoreManager scoreManager;
    public StoneManager stoneManager;

    public Slider powerSlider;
    public float minPower = 10f;
    public float maxPower = 20f;

    public GameObject projectileArrow;
    public float angleIncrement = 20f;
    public float powerIncrement = 20f;

    private float currentPower;
    private float currentAngle;

    public string arrowTag = "AngleArrow";

    private Vector3 currentDirection;

    private GameObject previewStone;

    private bool isUpdatePreview = true;
    private bool isLeftndRight = true;

    private bool isReady = true;
    public enum ThrowState { Aiming, Charging, ReadyToThrow } // 둘 다 퍼블릭변경
    public ThrowState currentState = ThrowState.Aiming;

    private int weight = 2;



    private void Start()
    {
        powerSlider.minValue = minPower;
        powerSlider.maxValue = maxPower;
        currentPower = minPower;
        powerSlider.value = currentPower;

        if (powerSlider != null)
        {
            powerSlider.gameObject.SetActive(false);
        }

        if (stoneManager != null)
        {
            CreatePreviewStone();
        }
        else
        {
            Debug.LogError("스톤 매니저가 널임!");
        }

        UpdateBallCountText();

        StoneManager.OnStoneTypeChanged += CreatePreviewStone;
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

        if (stoneManager.GetCurrentStoneCount(stoneManager.currentStoneType) == 0)
        {
            return; // 볼 개수가 모자라다면 조준도 하지 못하게
        }
        if (!isReady)
        {
            return; // 코루틴 시간
        }


        switch (currentState)
        {
            case ThrowState.Aiming:
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    isUpdatePreview = false;

                    if (currentBallCount < stoneManager.maxBalls) // 슬라이더 비활성화 / 피드백 반영
                    {
                        currentState = ThrowState.Charging;

                        if (powerSlider != null)
                        {
                            powerSlider.gameObject.SetActive(true);
                        }
                    }
                }
                break;

            case ThrowState.Charging:
                if (Input.GetKeyUp(KeyCode.Space))
                {
                    // StartCoroutine(DisablePreviewUpdateForSeconds(2f));

                    isUpdatePreview = true;

                    if (currentBallCount < stoneManager.maxBalls) // 발사 가능한 경우
                    {
                        Throw(currentDirection);
                        currentState = ThrowState.Aiming;
                        CreatePreviewStone(); // 발사 후에만 새로운 previewStone을 생성, 다른곳 생성 x

                        currentPower = minPower; // 발사한뒤, 파워랑 슬라이더 초기화
                        powerSlider.value = currentPower;

                        if (powerSlider != null)
                        {
                            powerSlider.gameObject.SetActive(false);
                        }

                        currentBallCount++;
                        UpdateBallCountText();

                        if (currentBallCount >= stoneManager.maxBalls)
                        { StartCoroutine(EndGame()); }

                    }
                    else
                    {
                        if (currentState == ThrowState.Charging)
                        {
                            currentState = ThrowState.Aiming;
                            powerSlider.gameObject.SetActive(false);  // 슬라이더 비활성화
                        }

                    }
                }
                break;
        }

        // 돌 타입 변경 테스트
        //if (Input.GetKeyDown(KeyCode.Alpha1))
        //{
        //    stoneManager.ChangeStoneToType(0);
        //    CreatePreviewStone();
        //}
        //else if (Input.GetKeyDown(KeyCode.Alpha2))
        //{
        //    stoneManager.ChangeStoneToType(1);
        //    CreatePreviewStone();
        //}
        //else if (Input.GetKeyDown(KeyCode.Alpha3))
        //{
        //    stoneManager.ChangeStoneToType(2);
        //    CreatePreviewStone();
        //}
        //else if (Input.GetKeyDown(KeyCode.Alpha4))
        //{
        //    stoneManager.ChangeStoneToType(3);
        //    CreatePreviewStone();
        //}
    }

    public void Throw(Vector3 currentDirection)
    {

        if (stoneManager.GetCurrentStoneCount(stoneManager.currentStoneType) > 0)
        {
            // CreatePreviewStone(); // 아래 위치에서 변경

            if (previewStone != null)
            {
                Rigidbody rb = previewStone.GetComponent<Rigidbody>();
                rb.AddForce(currentDirection * currentPower, ForceMode.Impulse); // 수정된 부분

                MagneticStone magneticStone = previewStone.GetComponent<MagneticStone>(); // 발사 상태알림
                if (magneticStone != null)
                {
                    magneticStone.FireStone();
                }
                ExplosiveStone explosiveStone = previewStone.GetComponent<ExplosiveStone>(); // 발사 상태알림
                if (explosiveStone != null)
                {
                    explosiveStone.FireStone();
                }

                previewStone = null;  // 기존 참조 제거

                if (stoneManager != null)  // stoneManager가 null이 아닌 경우만 실행
                {
                    stoneManager.UseStone(stoneManager.currentStoneType);  // 수정된 부분
                }

                StartCoroutine(AllResetAndSetting(3.0f));  // 올리셋 및 세팅 - 슬라이더, 앵글애로우, 프리뷰스톤, 스페이스

            }
        }
        else
        {
            Debug.Log("스톤 부족!!!!");
        }
    }


    private IEnumerator AllResetAndSetting(float seconds)
    {
        isReady = false;

        // 발사 직후 초기화 및 비활성화
        if (powerSlider != null)
        {
            powerSlider.gameObject.SetActive(false);
        }

        if (projectileArrow != null)
        {
            projectileArrow.SetActive(false);
        }

        if (previewStone != null)
        {
            Destroy(previewStone);
        }

        // 4초 대기
        yield return new WaitForSeconds(seconds);

        isReady = true;

        powerSlider.gameObject.SetActive(true);
        projectileArrow.SetActive(true);
        

        // 초기값 설정
        currentState = ThrowState.Aiming;
        currentPower = minPower;
        powerSlider.value = currentPower;


        //CreatePreviewStone(); // 새로운 previewStone 생성

        //// 모든 요소 다시 활성화
        //if (powerSlider != null)
        //{
        //    powerSlider.gameObject.SetActive(true);
        //}

        //if (projectileArrow != null)
        //{
        //    projectileArrow.SetActive(true);
        //}

        UpdateBallCountText();
    }



    private void UpdateAngle()
    {
        currentAngle += (isLeftndRight ? 1 : -1) * angleIncrement * Time.deltaTime;

        if (currentAngle >= 45f || currentAngle <= -45f)
        {
            isLeftndRight = !isLeftndRight;
        }

        currentDirection = Quaternion.Euler(0, transform.eulerAngles.y + currentAngle, 0) * Vector3.forward;

        if (projectileArrow != null)
        {
            projectileArrow.transform.position = transform.position + currentDirection;
            projectileArrow.transform.rotation = Quaternion.LookRotation(currentDirection);
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

        // previewStone = stoneManager.CreateStone(transform.position, Quaternion.identity);
        previewStone = stoneManager.CreateStone(transform.position + Vector3.up, Quaternion.identity);
        Rigidbody rb = previewStone.GetComponent<Rigidbody>();

    }

    public void UpdatePreviewStonePosition(Vector3 newPosition)
    {
        if (isUpdatePreview)
        {
            if (previewStone != null)
            {
                previewStone.transform.position = newPosition;
            }
        }
    }

    IEnumerator EndGame()
    {
        if (playerMovement != null)
        {
            playerMovement.enabled = false;
        }

        yield return new WaitForSeconds(9);  // 8초 대기 임시

        if (scoreManager != null)
        {

            if (scoreManager.totalScore >= 15)  // 점수가 넘으면 클리어 씬
            {
                SceneManager.LoadScene("ClearScene"); // 넥스트기능이 포함이 되어있으니까
            }
            else
            {
                SceneManager.LoadScene("DefeatScene");
            }
        }

    }

    //private IEnumerator DisablePreviewUpdateForSeconds(float seconds)
    //{
    //    isUpdatePreview = false;
    //    yield return new WaitForSeconds(seconds);
    //    isUpdatePreview = true;
    //}

    private void UpdateBallCountText()
    {
        if (ballCountText != null)
        {
            int updateBallCount = stoneManager.maxBalls - currentBallCount; // 남은 볼 수 계산
            ballCountText.text = "Ball Count: " + updateBallCount.ToString();
        }
    }
    private void OnDestroy()
    {
        StoneManager.OnStoneTypeChanged -= CreatePreviewStone;  // 이벤트 구독 취소
    }

}