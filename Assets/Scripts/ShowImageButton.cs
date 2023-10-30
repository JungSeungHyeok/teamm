using UnityEngine;

public class ShowImageButton : MonoBehaviour
{
    public GameObject canvas1;
    public GameObject canvas2;
    public GameObject canvas3;
    public GameObject canvas4;

    public void ToggleCanvas(int canvasNumber)
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            return;
        }

        //Debug.Log("ToggleCanvas called with canvasNumber: " + canvasNumber);

        GameObject clickedCanvas = null;
        bool isClickedCanvasActive = false;

        switch (canvasNumber)
        {
            case 1:
                clickedCanvas = canvas1;
                break;
            case 2:
                clickedCanvas = canvas2;
                break;
            case 3:
                clickedCanvas = canvas3;
                break;
            case 4:
                clickedCanvas = canvas4;
                break;
            default:
                break;
        }

        if (clickedCanvas != null)
        {
            isClickedCanvasActive = clickedCanvas.activeSelf;
        }

        // 활성화된 캔버스 개수 체크
        int activeCanvasCount = 0;
        if (canvas1.activeSelf) activeCanvasCount++;
        if (canvas2.activeSelf) activeCanvasCount++;
        if (canvas3.activeSelf) activeCanvasCount++;
        if (canvas4.activeSelf) activeCanvasCount++;

        //if (activeCanvasCount >= 1) { Time.timeScale = 0; }
        //else { Time.timeScale = 1; }

        
        if (isClickedCanvasActive && activeCanvasCount >= 2)
        {
            TurnOffAllCanvases();
            return;
        }

        if (clickedCanvas != null)
        {
            clickedCanvas.SetActive(!clickedCanvas.activeSelf);
        }
    }

    public void TurnOffAllCanvases()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            return;
        }

        canvas1.SetActive(false);
        canvas2.SetActive(false);
        canvas3.SetActive(false);
        canvas4.SetActive(false);

        Time.timeScale = 1;
    }
}
