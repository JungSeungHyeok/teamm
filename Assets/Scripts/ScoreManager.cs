using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    [System.Serializable]
    public class ScoreLineInfo
    {
        public Transform position;  // 라인 위치
        public int score;  // 해당 라인의 점수
    }

    public List<ScoreLineInfo> scoreLines;  // 인스펙터에서 설정 가능

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

                    if (distanceToLine < 1f)  // 거리 임계값 설정 가능
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
//    public List<Transform> targetPositions; // 변경: Transform 대신 List<Transform>
//    public float stopThreshold = 0.1f;
//    public float distanceThreshold = 1f;
//    public TextMeshProUGUI scoreText;

//    // 추가: 각 위치에 따른 점수
//    public Dictionary<Transform, int> targetScores;

//    private int score = 0;

//    private void Start()
//    {
//        UpdateScoreText();
//        // 추가: 초기 점수 설정
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

//            // 변경: 모든 타겟에 대해 체크
//            foreach (Transform targetPosition in targetPositions)
//            {
//                float distanceToTarget = Vector3.Distance(stone.transform.position, targetPosition.position);

//                if (speed < stopThreshold && distanceToTarget < distanceThreshold)
//                {
//                    score += targetScores[targetPosition];  // 변경: 점수를 딕셔너리에서 가져옴
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
