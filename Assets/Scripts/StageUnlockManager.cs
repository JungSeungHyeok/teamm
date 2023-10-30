using UnityEngine;
using UnityEngine.UI;

public class StageUnlockManager : MonoBehaviour
{
    public Button[] stageButtons;

    void Start()
    {
        int maxUnlockedStage = Stage.LoadMaxUnlockedStage();
        for (int i = 0; i < stageButtons.Length; i++)
        {
            if (i >= maxUnlockedStage)
            {
                stageButtons[i].interactable = false;
            }
            else
            {
                stageButtons[i].interactable = true;
            }
        }
    }

}
