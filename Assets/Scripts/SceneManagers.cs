using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagers : MonoBehaviour
{
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
        lastScene = "Stage1-1"; // �����ϱ� ���� ������ ���� ������Ʈ
        SceneManager.LoadScene("Stage1-1");
    }

    public void OnRestartButtonClicked()
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
        Application.Quit(); // ���� ����
    }

    internal static object GetActiveScene()
    {
        throw new NotImplementedException();
    }
}
