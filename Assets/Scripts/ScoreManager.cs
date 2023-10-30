using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
//
public class ScoreManager : MonoBehaviour
{
    [System.Serializable]
    public class ScoreLineInfo
    {
        public BoxCollider boxCollider;
        public int score;
    }

    private int winScore;

    //public GameObject particleEffect;
    //private GameObject effectInstance;

    public List<ScoreLineInfo> scoreLines;
    public float stopThreshold = 500f;
    public TextMeshProUGUI scoreText;

    public int totalScore = 0;
    private Dictionary<GameObject, int> stoneCurrentScore = new Dictionary<GameObject, int>();

    private void Start()
    {
        UpdateScoreText();

        RectTransform canvasRect = GetComponentInParent<Canvas>().GetComponent<RectTransform>();

        Vector3 screenPoint = new Vector3(canvasRect.rect.xMin, canvasRect.rect.yMax, 0);

        Camera mainCamera = Camera.main;
        Vector3 worldPoint;
        RectTransformUtility.ScreenPointToWorldPointInRectangle(canvasRect, screenPoint, mainCamera, out worldPoint);

        winScore = Stage.GetStageWinScore(SceneManager.GetActiveScene().name);
    }

    private void FixedUpdate()
    {
        GameObject[] stones = GameObject.FindGameObjectsWithTag("Stone");
        GameObject[] StickyStone = GameObject.FindGameObjectsWithTag("StickyStone");
        GameObject[] shinyStones = GameObject.FindGameObjectsWithTag("ShinyStone");

        StoneScoreTypes(stones, 1);
        StoneScoreTypes(StickyStone, 1);
        StoneScoreTypes(shinyStones, 2);
    }

    private void StoneScoreTypes(GameObject[] stones, int multiple)
    {
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
                        currentStoneScore = line.score * multiple;
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
            string currentStage = Stage.CurrentStage();
            scoreText.text = "Score: " + totalScore + " " + "/ " + Stage.stageWinScores[Stage.CurrentStage()];
        }
    }

    public void UpdateTotalScore(int newScore)
    {
        totalScore += newScore;
        PlayerPrefs.SetInt("TotalScore", totalScore);
    }
}
