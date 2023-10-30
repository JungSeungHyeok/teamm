using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StageManager : MonoBehaviour
{
    public TextMeshProUGUI scoreText;  
    public int totalScore;

    private void Start()
    {
        Stage.LoadStageCount();
        UpdateScoreText();
    }
    private void UpdateScoreText()
    {
        if (scoreText != null)
        {
            string currentStage = Stage.CurrentStage();
            scoreText.text = "Score: " + totalScore + " " + "/ " + Stage.stageWinScores[currentStage];
        }
        else
        {
            //Debug.LogWarning("scoreText is null.");
        }
    }

    public void GoToNextStage()
    {
        PauseButton.isPaused = false;
        SceneManager.LoadScene(Stage.NextStage());
        Stage.SaveMaxUnlockedStage();
    }

    public void RestartSeclected()
    {
        SceneManager.LoadScene(Stage.CurrentStage());
        PauseButton.isPaused = false;
    }

    public void SelectStage(string stageName)
    {
        PauseButton.isPaused = false;
        Stage.SetStage(stageName);
        //Debug.Log("Current stageCount: " + Stage.stageCount);
        SceneManager.LoadScene(stageName);
    }

    public void SelectScene( )
    {
        PauseButton.isPaused = false;
        Time.timeScale = 1;
        SceneManager.LoadScene("SelectScene");
    }

    public void TitleScene()
    {
        PauseButton.isPaused = false;
        Time.timeScale = 1;
        SceneManager.LoadScene("TitleScene");
    }

    public void PreviousStage()
    {
        PauseButton.isPaused = false;
        Stage.PreviousStage();
        SceneManager.LoadScene(Stage.CurrentStage());
    }

    public void GameOver()
    {
        Application.Quit(); // 게임 종료
    }

}
