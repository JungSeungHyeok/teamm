using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BallButtonHandler : MonoBehaviour
{
    private StoneManager stoneManager;  // StoneManager 참조

    public Button stoneButton1;
    public Button stoneButton2;
    public Button stoneButton3;

    public TextMeshProUGUI countText1;
    public TextMeshProUGUI countText2;
    public TextMeshProUGUI countText3;

    public void Start()
    {
        // StoneManager 인스턴스를 찾아 참조를 설정
        stoneManager = FindObjectOfType<StoneManager>();

        stoneButton1.onClick.AddListener(() => HandleButtonClick(0));
        stoneButton2.onClick.AddListener(() => HandleButtonClick(1));
        stoneButton3.onClick.AddListener(() => HandleButtonClick(2));

    }

    public void Update()
    {
        if (stoneManager != null)
        {
            countText1.text = stoneManager.GetCurrentStoneCount(0).ToString();
            countText2.text = stoneManager.GetCurrentStoneCount(1).ToString();
            countText3.text = stoneManager.GetCurrentStoneCount(2).ToString();
        }
    }

    public void HandleButtonClick(int stoneType)
    {
        if (stoneManager != null)
        {
            stoneManager.ChangeStoneToType(stoneType);
        }
    }
}
