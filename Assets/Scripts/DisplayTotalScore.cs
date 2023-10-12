using TMPro;
using UnityEngine;

public class DisplayTotalScore : MonoBehaviour
{
    public TextMeshProUGUI totalScoreText;
    public ScoreManager scoreManager;

    private void Start()
    {
        int savedScore = PlayerPrefs.GetInt("TotalScore", 0);

        if (totalScoreText != null)
        {
            totalScoreText.text = "Total Score: " + savedScore;
        }
    }

    //public void Restart()
    //{
    //    StageManager.currentStage
    //}
}
