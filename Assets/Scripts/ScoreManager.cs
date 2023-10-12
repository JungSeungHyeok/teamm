using System.Collections.Generic;
using TMPro;
using UnityEngine;
//
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

    public int totalScore = 0;
    private Dictionary<GameObject, int> stoneCurrentScore = new Dictionary<GameObject, int>();

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

            if (isBelowThreshold)
            {
                int currentStoneScore = 0;

                foreach (ScoreLineInfo line in scoreLines)
                {
                    bool isInside = line.boxCollider.bounds.Contains(stone.transform.position);

                    if (isInside)
                    {
                        currentStoneScore = line.score;
                        break;
                    }
                }

                if (!stoneCurrentScore.ContainsKey(stone))
                {
                    stoneCurrentScore[stone] = 0;
                }

                totalScore += currentStoneScore - stoneCurrentScore[stone];
                stoneCurrentScore[stone] = currentStoneScore;
            }
        }

        UpdateScoreText();
        UpdateTotalScore(0);
    }

    private void UpdateScoreText()
    {
        if (scoreText != null)
        {
            scoreText.text = "Score: " + totalScore;
        }
    }

    public void UpdateTotalScore(int newScore) // 다른 씬에 점수 전달
    {
        totalScore += newScore;
        PlayerPrefs.SetInt("TotalScore", totalScore);
    }
}
