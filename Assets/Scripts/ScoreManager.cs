using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    [System.Serializable]
    public class ScoreLineInfo
    {
        public Transform position;  // ���� ��ġ
        public int score;  // �ش� ������ ����
    }

    public List<ScoreLineInfo> scoreLines;  // �ν����Ϳ��� ���� ����

    public float stopThreshold = 0.1f;
    public TextMeshProUGUI scoreText;

    private int totalScore = 0;

    private void Start()
    {
        UpdateScoreText();
    }

    private void FixedUpdate()
    {
        GameObject[] stones = GameObject.FindGameObjectsWithTag("Stone");

        foreach (GameObject stone in stones)
        {
            Rigidbody stoneRb = stone.GetComponent<Rigidbody>();

            if (stoneRb == null)
            {
                Debug.LogWarning("Rigidbody not found on Stone");
                continue;
            }

            float speed = stoneRb.velocity.magnitude;

            if (speed < stopThreshold)
            {
                foreach (ScoreLineInfo line in scoreLines)
                {
                    float distanceToLine = Vector3.Distance(stone.transform.position, line.position.position);

                    if (distanceToLine < 1f)  // �Ÿ� �Ӱ谪 ���� ����
                    {
                        totalScore += line.score;
                        Destroy(stone);
                        UpdateScoreText();
                        break;
                    }
                }
            }
        }
    }

    private void UpdateScoreText()
    {
        if (scoreText != null)
        {
            scoreText.text = "Score: " + totalScore;
        }
        else
        {
            Debug.LogWarning("ScoreText is not assigned");
        }
    }
}














//using System.Collections.Generic;
//using TMPro;
//using UnityEngine;

//public class ScoreManager : MonoBehaviour
//{
//    public List<Transform> targetPositions; // ����: Transform ��� List<Transform>
//    public float stopThreshold = 0.1f;
//    public float distanceThreshold = 1f;
//    public TextMeshProUGUI scoreText;

//    // �߰�: �� ��ġ�� ���� ����
//    public Dictionary<Transform, int> targetScores;

//    private int score = 0;

//    private void Start()
//    {
//        UpdateScoreText();
//        // �߰�: �ʱ� ���� ����
//        targetScores = new Dictionary<Transform, int>();
//        targetScores[targetPositions[0]] = 5;
//        targetScores[targetPositions[1]] = 3;
//        targetScores[targetPositions[2]] = 1;
//    }

//    private void FixedUpdate()
//    {
//        GameObject[] stones = GameObject.FindGameObjectsWithTag("Stone");

//        foreach (GameObject stone in stones)
//        {
//            Rigidbody stoneRb = stone.GetComponent<Rigidbody>();

//            if (stoneRb == null)
//            {
//                Debug.LogWarning("Rigidbody not found on Stone");
//                continue;
//            }

//            float speed = stoneRb.velocity.magnitude;

//            // ����: ��� Ÿ�ٿ� ���� üũ
//            foreach (Transform targetPosition in targetPositions)
//            {
//                float distanceToTarget = Vector3.Distance(stone.transform.position, targetPosition.position);

//                if (speed < stopThreshold && distanceToTarget < distanceThreshold)
//                {
//                    score += targetScores[targetPosition];  // ����: ������ ��ųʸ����� ������
//                    Destroy(stone);
//                    UpdateScoreText();
//                }
//            }
//        }
//    }

//    private void UpdateScoreText()
//    {
//        if (scoreText != null)
//        {
//            scoreText.text = "Score: " + score;
//        }
//        else
//        {
//            Debug.LogWarning("ScoreText is not assigned");
//        }
//    }
//}
