using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BallButtonHandler : MonoBehaviour
{
    private StoneManager stoneManager;  // StoneManager 참조

    private StoneControler stoneControler;

    string[] stoneTypeLabels =
        { "Basic Stone", "Magnetic Stone", "Explosive Stone", "Shiny Stone", "Sticky Stone" };

    public Button stoneButton1;
    public Button stoneButton2;
    public Button stoneButton3;
    public Button stoneButton4;
    public Button stoneButton5;

    public TextMeshProUGUI countText1;
    public TextMeshProUGUI countText2;
    public TextMeshProUGUI countText3;
    public TextMeshProUGUI countText4;
    public TextMeshProUGUI countText5;

    public void Start()
    {
        stoneManager = FindObjectOfType<StoneManager>(); // 인스턴스 참조
        stoneControler = FindObjectOfType<StoneControler>();

        stoneButton1.onClick.AddListener(() => HandleButtonClick(0));
        stoneButton2.onClick.AddListener(() => HandleButtonClick(1));
        stoneButton3.onClick.AddListener(() => HandleButtonClick(2));
        stoneButton4.onClick.AddListener(() => HandleButtonClick(3));
        stoneButton5.onClick.AddListener(() => HandleButtonClick(4));
    }

    public void Update()
    {
        if (stoneManager != null)
        {
            for (int stoneType = 0; stoneType < 5; stoneType++)
            {
                int stoneCount = stoneManager.GetCurrentStoneCount(stoneType);

                string stoneTypeName = stoneTypeLabels[stoneType];
                UpdateCountText(stoneType, stoneCount, stoneTypeName);
            }
        }
    }

    private string GetStoneTypeName(int stoneType)
    {
        return "Type" + (stoneType + 1);
    }

    private void UpdateCountText(int stoneType, int stoneCount, string stoneTypeName)
    {
        // 해당 돌 타입에 대한 텍스트 업데이트
        switch (stoneType)
        {
            case 0:
                countText1.text = stoneTypeName + ": " + stoneCount;
                break;
            case 1:
                countText2.text = stoneTypeName + ": " + stoneCount;
                break;
            case 2:
                countText3.text = stoneTypeName + ": " + stoneCount;
                break;
            case 3:
                countText4.text = stoneTypeName + ": " + stoneCount;
                break;
            case 4:
                countText5.text = stoneTypeName + ": " + stoneCount;
                break;
        }
    }

    public void HandleButtonClick(int stoneType)
    {
        if (stoneControler && !stoneControler.isReady) { return; }

        if (stoneManager != null)
        {
            stoneManager.ChangeStoneToType(stoneType);
        }
    }
}
