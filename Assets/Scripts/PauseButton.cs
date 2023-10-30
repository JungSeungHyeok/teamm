using UnityEngine;

public class PauseButton : MonoBehaviour
{
    public GameObject popupCanvas; 
    public static bool isPaused = false; 

    public void TogglePause()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            return;
        }

        isPaused = !isPaused;

        if (isPaused)
        {
            Time.timeScale = 0;
            popupCanvas.SetActive(true);
        }
        else
        {
            Time.timeScale = 1;
            popupCanvas.SetActive(false);
        }
    }

    public void ToggleContinue()
    {
        isPaused = false;
        Time.timeScale = 1;
        popupCanvas.SetActive(false);
    }
}
