using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class StoneManager : MonoBehaviour
{
    public delegate void StoneTypeChanged();
    public static event StoneTypeChanged OnStoneTypeChanged;

    public List<GameObject> stonePrefabs;
    public GameObject currentStonePrefab;

    public Dictionary<int, int> stoneCounts = new Dictionary<int, int>();
    
    public List<Button> stoneButtons;

    public List<TextMeshProUGUI> stoneCountTexts;

    public int maxBalls = 5; // 기본 5개

    public int currentStoneType = 0;
    public int currentStoneIndex = 0;

    public int remainingBalls;

    public int GetCurrentStoneCount(int type)
    {
        if (stoneCounts.ContainsKey(type)) // 해당 타입의 현재 돌의 개수를 반환
        {
            return stoneCounts[type];
        }
        return 0;
    }

    public bool IsStoneAllowed(int index)
    {
        return GetCurrentStoneCount(index) <= maxBalls;
    }

    public void Start()
    {
        SetStoneForStage(SceneManager.GetActiveScene().name);
        remainingBalls = maxBalls;
    }

    public void SetStoneForStage(string stageName)
    {
        if (stageName == "Stage1-1")
        {
            stoneCounts[0] = 4;
            stoneCounts[1] = 1;
            stoneCounts[2] = 0;

            maxBalls = 5;
        }
        // 다른 스테이지일 때의 설정
        else if (stageName == "Stage1-2")
        {
            stoneCounts[0] = 4;
            stoneCounts[1] = 0;
            stoneCounts[2] = 1;

            maxBalls = 5;
        }

        UpdateUI();
    }

    void UpdateUI()
    {
        for (int i = 0; i < stoneButtons.Count; i++)
        {
            if (IsStoneAllowed(i))
            {
                stoneButtons[i].interactable = stoneCounts.ContainsKey(i) && stoneCounts[i] > 0;
            }
            else
            {
                stoneButtons[i].interactable = false;
            }

            // 돌의 개수를 UI에 표시
            if (stoneCounts.ContainsKey(i))
            {
                stoneCountTexts[i].text = stoneCounts[i].ToString();
            }
            else
            {
                stoneCountTexts[i].text = "0";
            }
        }
    }

    public void UseStone(int type)
    {
        if (stoneCounts.ContainsKey(type) && stoneCounts[type] > 0)
        {
            stoneCounts[type]--;
            remainingBalls--;  // 볼을 사용했으므로 남아있는 볼의 개수 감소
        }
        else
        {
            Debug.Log("스톤 부족!!!!");
        }
    }

    public void ChangeStoneToType(int newType)
    {
        if (newType >= 0 && newType < stonePrefabs.Count)
        {
            currentStoneType = newType;
            currentStonePrefab = stonePrefabs[currentStoneType];
            UpdateUI();

            OnStoneTypeChanged?.Invoke();
        }
    }

    public GameObject CreateStone(Vector3 position, Quaternion rotation)
    {
        GameObject stone = Instantiate(currentStonePrefab, position, rotation);
        return stone;
    }

    public void OnStoneUiClicked(int type)
    {
        ChangeStoneToType(type);   
    }

}

