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
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        currentState = GameState.Playing;
    }

    public void TestSelectScene()
    {
        SceneManager.LoadScene("SelectScene");
    }

    public void EndGame()
    {
        currentState = GameState.GameOver;
    }

    //// ���ӵ��� �����(R) �׽�Ʈ �ڵ�
    //private void Update()
    //{
    //    if (Input.GetKeyDown(KeyCode.R))
    //    {
    //        TestSelectScene();

    //        // RestartGame();
    //    }
    //}
}
