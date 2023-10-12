using UnityEngine;
using UnityEngine.SceneManagement;

public class StageManager : MonoBehaviour
{
    public void GoToNextStage()
    {
        SceneManager.LoadScene(Stage.NextStage());
    }

    public void RestartSeclected()
    {
        SceneManager.LoadScene(Stage.CurrentStage());
    }
    public void GameOver()
    {
        Application.Quit(); // 게임 종료
    }
}
