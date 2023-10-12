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

    public int maxBalls = 5; // �⺻ 5��

    public int currentStoneType = 0;
    public int currentStoneIndex = 0;

    public int remainingBalls;

    public int GetCurrentStoneCount(int type)
    {
        if (stoneCounts.ContainsKey(type)) // �ش� Ÿ���� ���� ���� ������ ��ȯ
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
        // �ٸ� ���������� ���� ����
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

            // ���� ������ UI�� ǥ��
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
            remainingBalls--;  // ���� ��������Ƿ� �����ִ� ���� ���� ����
        }
        else
        {
            Debug.Log("���� ����!!!!");
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

