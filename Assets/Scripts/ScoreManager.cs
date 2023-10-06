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
        List<GameObject> stonesToDestroy = new List<GameObject>();

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

                // 디버그
                //Debug.Log("IsInside: " + isInside + ", IsBelowThreshold: " + isBelowThreshold + ", Stone: " + stone.name);

                if (!stoneInside.ContainsKey(stone))
                {
                    stoneInside[stone] = false;
                }

                if (isInside && isBelowThreshold)
                {
                    if (!stoneInside[stone])
                    {
                        int scoreToAdd = line.score;
                        // 디버그
                        //Debug.Log("Is Shiny Stone: " + IsShinyStone(stone));

                        if (IsShinyStone(stone))
                        {
                            scoreToAdd *= 2;
                        }

                        totalScore += scoreToAdd;
                        stonesToDestroy.Add(stone);
                        //stoneInside[stone] = true;  // 이 부분이 문제일 수 있음
                        break;
                    }

                     stoneInside[stone] = true;  // 여기로 이동
                }
                else
                {
                    stoneInside[stone] = false; // 이 부분 추가
                }
            }
        }

        foreach (var stone in stonesToDestroy)
        {
            Destroy(stone);
            stoneInside.Remove(stone); // 상태 업데이트
        }

        if (stonesToDestroy.Count > 0)
        {
            UpdateScoreText();
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

