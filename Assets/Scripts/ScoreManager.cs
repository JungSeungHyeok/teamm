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

                // �簢�� �ȿ� ���� �ӵ��� �Ӱ谪 ������ ��
                if (isInside && isBelowThreshold)
                {
                    if (!stoneInside[stone]) // ������ �ȿ� ������� �ʾҴٸ�
                    {
                        int scoreToAdd = line.score;  // �⺻ ������ ����

                        // ��¦���̸� ������ 2���
                        if (IsShinyStone(stone))
                        {
                            scoreToAdd *= 2;
                        }

                        totalScore += scoreToAdd;  // ���� �߰�

                        Destroy(stone);  // �� �ı�
                        UpdateScoreText();  // ���� ������Ʈ
                        stoneInside.Remove(stone);  // ���� ������Ʈ
                        break;  // �ݺ��� Ż��
                    }

                    stoneInside[stone] = true;  // �ȿ� ���ٴ� ���¸� true�� ����
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

