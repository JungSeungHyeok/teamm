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
                    stoneInside[stone] = false;  // ó�� ������ ���� ������ false�� ����
                }

                // �簢�� �ȿ� ���� �ӵ��� �Ӱ谪 ������ ��
                if (isInside && isBelowThreshold)
                {
                    stoneInside[stone] = true;  // �ȿ� ���ٴ� ���¸� true�� ����
                }
                // �簢���� ������ �� ���� �ʱ�ȭ
                else if (!isInside)
                {
                    stoneInside[stone] = false;  // �����ٴ� ���¸� false�� ����
                }

                // �簢�� �ȿ� �������� �� (�������� �ȿ� �� ���� �ʾҰ�, ������ �� ���� ��)
                if (stoneInside[stone] && isBelowThreshold)
                {
                    totalScore += line.score;
                    Destroy(stone);
                    UpdateScoreText();
                    stoneInside.Remove(stone);  // ���� �ı������� Dictionary���� ����
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

//                // ó������ �簢�� ������ ����, �ӵ��� �Ӱ谪 ������ ��
//                if (isInside && isBelowThreshold && !stoneInside[stone])
//                {
//                    totalScore += line.score;
//                    Destroy(stone);
//                    UpdateScoreText();
//                    break;
//                }
//                // �簢���� ������ �� ���� �ʱ�ȭ
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
