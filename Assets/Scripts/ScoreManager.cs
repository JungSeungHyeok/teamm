using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    [System.Serializable]
    public class ScoreLineInfo
    {
        public BoxCollider boxCollider;
        public int score;
    }

    public List<ScoreLineInfo> scoreLines;
    public float stopThreshold = 0.1f;
    public TextMeshProUGUI scoreText;

    private int totalScore = 0;
    private Dictionary<GameObject, bool> stoneInside = new Dictionary<GameObject, bool>();

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
                continue;
            }

            float speed = stoneRb.velocity.magnitude;
            bool isBelowThreshold = speed < stopThreshold;

            foreach (ScoreLineInfo line in scoreLines)
            {
                bool isInside = line.boxCollider.bounds.Contains(stone.transform.position);
                if (!stoneInside.ContainsKey(stone))
                {
                    stoneInside[stone] = false;  // 처음 등장한 돌은 무조건 false로 설정
                }

                // 사각형 안에 들어가고 속도가 임계값 이하일 때
                if (isInside && isBelowThreshold)
                {
                    stoneInside[stone] = true;  // 안에 들어갔다는 상태를 true로 설정
                }
                // 사각형을 나갔을 때 상태 초기화
                else if (!isInside)
                {
                    stoneInside[stone] = false;  // 나갔다는 상태를 false로 설정
                }

                // 사각형 안에 정착했을 때 (이전에는 안에 들어가 있지 않았고, 지금은 들어가 있을 때)
                if (stoneInside[stone] && isBelowThreshold)
                {
                    totalScore += line.score;
                    Destroy(stone);
                    UpdateScoreText();
                    stoneInside.Remove(stone);  // 돌을 파괴했으니 Dictionary에서 제거
                    break;
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
    }
}























//using System.Collections.Generic;
//using TMPro;
//using UnityEngine;

//public class ScoreManager : MonoBehaviour
//{
//    [System.Serializable]
//    public class ScoreLineInfo
//    {
//        public BoxCollider boxCollider;
//        public int score;
//    }

//    public List<ScoreLineInfo> scoreLines;
//    public float stopThreshold = 0.1f;
//    public TextMeshProUGUI scoreText;

//    private int totalScore = 0;
//    private Dictionary<GameObject, bool> stoneInside = new Dictionary<GameObject, bool>();

//    private void Start()
//    {
//        UpdateScoreText();
//    }

//    private void FixedUpdate()
//    {
//        GameObject[] stones = GameObject.FindGameObjectsWithTag("Stone");

//        foreach (GameObject stone in stones)
//        {
//            Rigidbody stoneRb = stone.GetComponent<Rigidbody>();
//            if (stoneRb == null)
//            {
//                continue;
//            }

//            float speed = stoneRb.velocity.magnitude;
//            bool isBelowThreshold = speed < stopThreshold;

//            foreach (ScoreLineInfo line in scoreLines)
//            {
//                bool isInside = line.boxCollider.bounds.Contains(stone.transform.position);
//                if (!stoneInside.ContainsKey(stone))
//                {
//                    stoneInside[stone] = isInside;
//                }

//                // 처음으로 사각형 안으로 들어가고, 속도가 임계값 이하일 때
//                if (isInside && isBelowThreshold && !stoneInside[stone])
//                {
//                    totalScore += line.score;
//                    Destroy(stone);
//                    UpdateScoreText();
//                    break;
//                }
//                // 사각형을 나갔을 때 상태 초기화
//                else if (!isInside && stoneInside[stone])
//                {
//                    stoneInside[stone] = false;
//                }
//            }
//        }
//    }

//    private void UpdateScoreText()
//    {
//        if (scoreText != null)
//        {
//            scoreText.text = "Score: " + totalScore;
//        }
//    }
//}
