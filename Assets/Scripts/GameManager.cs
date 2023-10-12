using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public enum GameState { Playing, GameOver }
    public GameState currentState = GameState.Playing;

    public static GameManager instance;

    private void Start()
    {
        Stage.SetStage(SceneManager.GetActiveScene().name);
    }

    private void Awake()
    {
        if (instance == null) // 싱글턴
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject); // 씬이 변경되어도 파괴x
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R)) // 재시작
        {
            RestartGame();
        }
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        currentState = GameState.Playing;
    }

    public void EndGame()
    {
        currentState = GameState.GameOver;
        // 점수 화면 표시 로직 나중에
    }
}
