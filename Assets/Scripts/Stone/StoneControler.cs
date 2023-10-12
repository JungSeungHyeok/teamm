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

    public PlayerMovement playerMovement; // �÷��̾� ������ ����

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
    public enum ThrowState { Aiming, Charging, ReadyToThrow } // �� �� �ۺ�����
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
            Debug.LogError("���� �Ŵ����� ����!");
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
            return; // �� ������ ���ڶ�ٸ� ���ص� ���� ���ϰ�
        }
        if (!isReady)
        {
            return; // �ڷ�ƾ �ð�
        }


        switch (currentState)
        {
            case ThrowState.Aiming:
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    isUpdatePreview = false;

                    if (currentBallCount < stoneManager.maxBalls) // �����̴� ��Ȱ��ȭ / �ǵ�� �ݿ�
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

                    if (currentBallCount < stoneManager.maxBalls) // �߻� ������ ���
                    {
                        Throw(currentDirection);
                        currentState = ThrowState.Aiming;
                        CreatePreviewStone(); // �߻� �Ŀ��� ���ο� previewStone�� ����, �ٸ��� ���� x

                        currentPower = minPower; // �߻��ѵ�, �Ŀ��� �����̴� �ʱ�ȭ
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
                            powerSlider.gameObject.SetActive(false);  // �����̴� ��Ȱ��ȭ
                        }

                    }
                }
                break;
        }

        // �� Ÿ�� ���� �׽�Ʈ
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
            // CreatePreviewStone(); // �Ʒ� ��ġ���� ����

            if (previewStone != null)
            {
                Rigidbody rb = previewStone.GetComponent<Rigidbody>();
                rb.AddForce(currentDirection * currentPower, ForceMode.Impulse); // ������ �κ�

                MagneticStone magneticStone = previewStone.GetComponent<MagneticStone>(); // �߻� ���¾˸�
                if (magneticStone != null)
                {
                    magneticStone.FireStone();
                }
                ExplosiveStone explosiveStone = previewStone.GetComponent<ExplosiveStone>(); // �߻� ���¾˸�
                if (explosiveStone != null)
                {
                    explosiveStone.FireStone();
                }

                previewStone = null;  // ���� ���� ����

                if (stoneManager != null)  // stoneManager�� null�� �ƴ� ��츸 ����
                {
                    stoneManager.UseStone(stoneManager.currentStoneType);  // ������ �κ�
                }

                StartCoroutine(AllResetAndSetting(3.0f));  // �ø��� �� ���� - �����̴�, �ޱ۾ַο�, �����佺��, �����̽�

            }
        }
        else
        {
            Debug.Log("���� ����!!!!");
        }
    }


    private IEnumerator AllResetAndSetting(float seconds)
    {
        isReady = false;

        // �߻� ���� �ʱ�ȭ �� ��Ȱ��ȭ
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

        // 4�� ���
        yield return new WaitForSeconds(seconds);

        isReady = true;

        powerSlider.gameObject.SetActive(true);
        projectileArrow.SetActive(true);
        

        // �ʱⰪ ����
        currentState = ThrowState.Aiming;
        currentPower = minPower;
        powerSlider.value = currentPower;


        //CreatePreviewStone(); // ���ο� previewStone ����

        //// ��� ��� �ٽ� Ȱ��ȭ
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

        yield return new WaitForSeconds(9);  // 8�� ��� �ӽ�

        if (scoreManager != null)
        {

            if (scoreManager.totalScore >= 15)  // ������ ������ Ŭ���� ��
            {
                SceneManager.LoadScene("ClearScene"); // �ؽ�Ʈ����� ������ �Ǿ������ϱ�
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
            int updateBallCount = stoneManager.maxBalls - currentBallCount; // ���� �� �� ���
            ballCountText.text = "Ball Count: " + updateBallCount.ToString();
        }
    }
    private void OnDestroy()
    {
        StoneManager.OnStoneTypeChanged -= CreatePreviewStone;  // �̺�Ʈ ���� ���
    }

}