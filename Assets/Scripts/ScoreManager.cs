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
    // ����Ʈ���� ��ųʸ��� ������

    public bool IsShinyStone(GameObject stone)
    {
        return stone.CompareTag("ShinyStone");// ��¦������ Ȯ��
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

                // �����
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
                        // �����
                        //Debug.Log("Is Shiny Stone: " + IsShinyStone(stone));

                        if (IsShinyStone(stone))
                        {
                            scoreToAdd *= 2;
                        }

                        totalScore += scoreToAdd;
                        stonesToDestroy.Add(stone);
                        //stoneInside[stone] = true;  // �� �κ��� ������ �� ����
                        break;
                    }

                     stoneInside[stone] = true;  // ����� �̵�
                }
                else
                {
                    stoneInside[stone] = false; // �� �κ� �߰�
                }
            }
        }

        foreach (var stone in stonesToDestroy)
        {
            Destroy(stone);
            stoneInside.Remove(stone); // ���� ������Ʈ
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

