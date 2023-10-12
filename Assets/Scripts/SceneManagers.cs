using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagers : MonoBehaviour
{

    /// <summary>
    /// 추후 싱글톤 제거
    /// </summary>

    public static SceneManagers instance; 
    public string lastScene;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }


    public static int totalScore = 0;

    public void OnStartStageOneClicked()
    {
        lastScene = "Stage1-1"; // 시작하기 전에 마지막 씬을 업데이트
        SceneManager.LoadScene("Stage1-1");
    }

    //public void GameOver() // 게임에서 패배 조건이 발생했을 때 호출
    //{
    //    lastScene = SceneManager.GetActiveScene().name; // 현재 씬을 lastScene에 저장
    //    SceneManager.LoadScene("DefeatScene"); // 패배 씬 로드
    //}


    public void OnRestartButtonClicked() // 재시작 처리
    {
        if (!string.IsNullOrEmpty(lastScene))
        {
            SceneManager.LoadScene(lastScene);
        }
        else
        {
            // SceneManager.LoadScene("MainMenu");
        }
    }

    public void OnExitButtonClicked()
    {
        Application.Quit(); // 게임 종료
    }
}
