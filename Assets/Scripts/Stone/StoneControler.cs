using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class StoneControler : MonoBehaviour
{
    private int currentBallCount = 0;
    public TextMeshProUGUI ballCountText;

    public PlayerMovement playerMovement;

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

    public bool isReady = true;
    public enum ThrowState { Aiming, Charging, ReadyToThrow } // 둘 다 퍼블릭변경
    public ThrowState currentState = ThrowState.Aiming;

    private int weight = 2;

    public bool isSpaceUpDown = false;
    public bool isSpaceFirst = false;

    private void Start()
    {
        AudioSource audioSource = GetComponent<AudioSource>();
        if (audioSource != null)
        {
            audioSource.Stop();
        }

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
            //Debug.LogError("스톤 매니저가 널임!");
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
        if (PauseButton.isPaused) // Pause 상태 확인
        {
            return;
        }

        if (!isReady)
        {
            //Debug.Log("대기시간 임");
            return;
        }

        if (stoneManager.GetCurrentStoneCount(stoneManager.currentStoneType) == 0)
        {
            return; // 볼 개수가 모자라다면 조준도 하지못하게
        }

        //Debug.Log("현재파워: " + currentPower);

        switch (currentState)
        {
            case ThrowState.Aiming:
                if (Input.GetKeyDown(KeyCode.Space) || isSpaceUpDown)
                {
                    isUpdatePreview = false;

                    if (currentBallCount < stoneManager.maxBalls) // 슬라이더 비활성화 / 피드백 반영
                    {
                        currentState = ThrowState.Charging;

                        if (powerSlider != null)
                        {
                            powerSlider.gameObject.SetActive(true);
                            currentPower = minPower; // 초기화 위치 코루틴에서 옮김
                            powerSlider.value = currentPower;
                        }
                    }
                }
                break;

            case ThrowState.Charging:
                if (Input.GetKeyUp(KeyCode.Space) || !isSpaceUpDown)
                {
                    isUpdatePreview = true;

                    if (currentBallCount < stoneManager.maxBalls)
                    {
                        Throw(currentDirection);
                        currentState = ThrowState.Aiming;

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
                            powerSlider.gameObject.SetActive(false);
                        }
                    }
                }
                break;
        }
    }

    public void Throw(Vector3 currentDirection)
    {
        if (stoneManager.GetCurrentStoneCount(stoneManager.currentStoneType) > 0)
        {
            if (previewStone != null)
            {
                Rigidbody rb = previewStone.GetComponent<Rigidbody>();
                rb.isKinematic = false;
                rb.AddForce(currentDirection * currentPower, ForceMode.Impulse);

                MagneticStone magneticStone = previewStone.GetComponent<MagneticStone>();
                if (magneticStone != null)
                {
                    magneticStone.FireStone();
                }
                ExplosiveStone explosiveStone = previewStone.GetComponent<ExplosiveStone>();
                if (explosiveStone != null)
                {
                    explosiveStone.FireStone();
                }
                StickyStone stickyStone = previewStone.GetComponent<StickyStone>();
                if (stickyStone != null)
                {
                    stickyStone.FireStone();
                }

                previewStone = null;

                if (stoneManager != null)
                {
                    stoneManager.UseStone(stoneManager.currentStoneType);
                }

                StartCoroutine(AllResetAndSetting(1.0f));
            }
        }
        else
        {
            //Debug.Log("스톤 부족!!!!");
        }
    }

    private IEnumerator AllResetAndSetting(float seconds)
    {
        isReady = false;

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

        // 3초 대기
        yield return new WaitForSeconds(seconds);

        isReady = true;
        projectileArrow.SetActive(true);

        CreatePreviewStone();
        currentState = ThrowState.Aiming;
    }

    private void UpdateAngle()
    {
        currentAngle += (isLeftndRight ? 1 : -1) * angleIncrement * Time.deltaTime;
        if (currentAngle >= 90f || currentAngle <= -90f)
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

        if (currentPower >= maxPower || currentPower < minPower)
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

        previewStone = stoneManager.CreateStone(transform.position + Vector3.up, Quaternion.identity);
        Rigidbody rb = previewStone.GetComponent<Rigidbody>();
        rb.isKinematic = true;

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

        yield return new WaitForSeconds(10f);

        if (scoreManager != null)
        {
            string currentStage = Stage.CurrentStage(); // 현재 스테이지 가져오기
            int winScore = Stage.stageWinScores.ContainsKey(currentStage) ? Stage.stageWinScores[currentStage] : 15; // 스테이지별 승리 점수 가져오기

            if (scoreManager.totalScore >= winScore)  // 점수가 넘으면 클리어 씬
            {
                SceneManager.LoadScene("ClearScene");
            }
            else
            {
                SceneManager.LoadScene("DefeatScene");
            }
        }
    }

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
        StoneManager.OnStoneTypeChanged -= CreatePreviewStone;  // 이벤트 취소
    }

    public void FirstThrowButtonDown()
    {
        Debug.Log("FirstThrowButtonDown 호출됨");
        isSpaceUpDown = true;
        isSpaceFirst = false;

    }

    public void SecendThrowButtonDown()
    {
        
            isSpaceUpDown = false;
            isSpaceFirst = true;
        
    }




    //public void SpaceThrowButtonDown()
    //{

    //    if (currentState == ThrowState.Aiming)
    //    {
    //        isSpaceUpDown = true;
    //        isSpaceFirst = false;
    //    }

    //}

    //public void SpaceThrowButtonUp()
    //{
    //    if (currentState == ThrowState.Charging)
    //    {
    //        isSpaceUpDown = false;
    //        isSpaceFirst = true;
    //    }

    //}

}