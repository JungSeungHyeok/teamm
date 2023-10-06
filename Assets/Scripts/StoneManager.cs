// StoneManager.cs
using System.Collections.Generic;
using UnityEngine;

public class StoneManager : MonoBehaviour
{
    public List<GameObject> stonePrefabs;
    public GameObject currentStonePrefab;
    private int currentStoneIndex = 0;

    public void ChangeStoneToType(int index)
    {
        if (index < 0 || index >= stonePrefabs.Count)
        {
            return;
        }

        currentStoneIndex = index;
        currentStonePrefab = stonePrefabs[currentStoneIndex];
    }

    public GameObject CreateStone(Vector3 position, Quaternion rotation)
    {
        GameObject stone = Instantiate(currentStonePrefab, position, rotation);
        return stone;
    }
}
