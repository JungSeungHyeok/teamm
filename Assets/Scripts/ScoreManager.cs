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
    // 리스트보다 딕셔너리가 나은듯

    public bool IsShinyStone(GameObject stone)
    {
        return stone.CompareTag("ShinyStone");// 반짝돌인지 확인
    }

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
                    stoneInside[stone] = false;
                }

                // 사각형 안에 들어가고 속도가 임계값 이하일 때
                if (isInside && isBelowThreshold)
                {
                    if (!stoneInside[stone]) // 이전에 안에 들어있지 않았다면
                    {
                        int scoreToAdd = line.score;  // 기본 점수로 시작

                        // 반짝돌이면 점수를 2배로
                        if (IsShinyStone(stone))
                        {
                            scoreToAdd *= 2;
                        }

                        totalScore += scoreToAdd;  // 점수 추가

                        Destroy(stone);  // 돌 파괴
                        UpdateScoreText();  // 점수 업데이트
                        stoneInside.Remove(stone);  // 상태 업데이트
                        break;  // 반복문 탈출
                    }

                    stoneInside[stone] = true;  // 안에 들어갔다는 상태를 true로 설정
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

