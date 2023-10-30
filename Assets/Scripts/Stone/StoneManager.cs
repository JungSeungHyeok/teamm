using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class StoneManager : MonoBehaviour
{
    StoneControler stoneControler;

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
        if (stoneCounts.ContainsKey(type))
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

        currentStoneType = 0;

        if (stonePrefabs.Count > 0 && currentStoneType < stonePrefabs.Count)
        {
            // Debug.Log("스톤매니저 스타트 if문");
            currentStonePrefab = stonePrefabs[currentStoneType];
            OnStoneTypeChanged?.Invoke();  // 초기 스톤 타입에 따른 이벤트 발생
        }

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
        else if (stageName == "Stage1-2")
        {
            stoneCounts[0] = 4;
            stoneCounts[1] = 0;
            stoneCounts[2] = 1;

            maxBalls = 5;
        }
        else if (stageName == "Stage1-3")
        {
            stoneCounts[0] = 3;
            stoneCounts[1] = 0;
            stoneCounts[2] = 2;

            maxBalls = 5;
        }
        else if (stageName == "Stage1-4")
        {
            stoneCounts[0] = 3;
            stoneCounts[1] = 1;
            stoneCounts[2] = 2;

            maxBalls = 6;
        }
        else if (stageName == "Stage1-5")
        {
            stoneCounts[0] = 0;
            stoneCounts[1] = 1;
            stoneCounts[2] = 0;
            stoneCounts[3] = 2;
            stoneCounts[4] = 2;
            maxBalls = 5;
        }
        else if (stageName == "Stage1-6")
        {
            stoneCounts[0] = 2;
            stoneCounts[1] = 1;
            stoneCounts[2] = 0;
            stoneCounts[3] = 0;
            stoneCounts[4] = 2;
            maxBalls = 5;
        }
        else if (stageName == "Stage1-7")
        {
            stoneCounts[0] = 2;
            stoneCounts[1] = 1;
            stoneCounts[2] = 0;
            stoneCounts[3] = 0;
            stoneCounts[4] = 2;
            maxBalls = 5;
        }
        else if (stageName == "Stage1-8")
        {
            stoneCounts[0] = 1;
            stoneCounts[1] = 1;
            stoneCounts[2] = 3;
            stoneCounts[3] = 0;
            stoneCounts[4] = 0;
            maxBalls = 5;
        }
        else if (stageName == "Stage1-9")
        {
            stoneCounts[0] = 2;
            stoneCounts[1] = 0;
            stoneCounts[2] = 0;
            stoneCounts[3] = 0;
            stoneCounts[4] = 3;
            maxBalls = 5;
        }
        else if (stageName == "Stage1-10")
        {
            stoneCounts[0] = 1;
            stoneCounts[1] = 0;
            stoneCounts[2] = 4;
            stoneCounts[3] = 0;
            stoneCounts[4] = 0;
            maxBalls = 5;
        }

        UpdateUI();
    }

    void UpdateUI()
    {
        for (int i = 0; i < stoneButtons.Count; i++)
        {
            if (i >= stoneCountTexts.Count) // 추가된 체크
            {
                break;
            }

            if (IsStoneAllowed(i))
            {
                stoneButtons[i].interactable = stoneCounts.ContainsKey(i) && stoneCounts[i] > 0;
            }
            else
            {
                stoneButtons[i].interactable = false;
            }

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
            remainingBalls--;
        }
        else
        {
            // Debug.Log("스톤 부족!!!!");
            return;
        }

        if (stoneCounts[type] == 0)
        {
            int nextType = FindNextAvailableStoneType(type);
            if (nextType != -1) // 함수호출 실패한경우 -1반환
            {
                ChangeStoneToType(nextType);
            }
            else
            {
                // Debug.Log("스톤 다 씀");
            }
        }
    }

    public int FindNextAvailableStoneType(int currentType)
    {
        int startIndex = (currentType + 1) % stonePrefabs.Count;
        int index = startIndex;

        do
        {
            if (GetCurrentStoneCount(index) > 0)
            {
                return index;
            }
            index = (index + 1) % stonePrefabs.Count;
        } while (index != startIndex);

        return -1;
    }

    public GameObject CreateStone(Vector3 position, Quaternion rotation)
    {
        GameObject stone = Instantiate(currentStonePrefab, position, rotation);
        return stone;
    }

    public void ChangeStoneToType(int newType)
    {
        // 대기시간때 타입변경x 추가
        // if (!stoneControler.isReady) { return; }

        if (newType >= 0 && newType < stonePrefabs.Count)
        {
            if (GetCurrentStoneCount(newType) == 0)
            {
                // Debug.Log("스톤 수가 0개임");
                return;
            }

            currentStoneType = newType;
            currentStonePrefab = stonePrefabs[currentStoneType];
            UpdateUI();

            OnStoneTypeChanged?.Invoke();
        }
    }

    public void OnStoneUiClicked(int type)
    {
        // if (!stoneControler.isReady) { return; }

        ChangeStoneToType(type);
    }

}

