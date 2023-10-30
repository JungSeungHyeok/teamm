using System.Collections.Generic;
using UnityEngine;

public static class Stage // 어떤 역할?
{
    public static int stageCount { get; private set; } = 1;

    public static Dictionary<string, int> stageWinScores = new Dictionary<string, int>
    {
        { "Stage1-1", 20 },
        { "Stage1-2", 17 },
        { "Stage1-3", 10 },
        { "Stage1-4", 15 },
        { "Stage1-5", 24 },
        { "Stage1-6", 15 },
        { "Stage1-7", 15 },
        { "Stage1-8", 15 },
        { "Stage1-9", 20 },
        { "Stage1-10", 15 },
    };

    public static string CurrentStage()
    {
        string result = stageCount switch
        {
            0 => "SelectScene",
            1 => "Stage1-1",
            2 => "Stage1-2",
            3 => "Stage1-3",
            4 => "Stage1-4",
            5 => "Stage1-5",
            6 => "Stage1-6",
            7 => "Stage1-7",
            8 => "Stage1-8",
            9 => "Stage1-9",
            10 => "Stage1-10",
            _ => "Stage1-1" // 기본값
        };

        return result;
    }
    
    public static string NextStage()
    {
        stageCount++;
        SaveStageCount();
        return CurrentStage();
    }

    public static void SetStage(string str)
    {
        stageCount = str switch
        {
            "SelectScene" => 0,
            "Stage1-1" => 1,
            "Stage1-2" => 2,
            "Stage1-3" => 3,
            "Stage1-4" => 4,
            "Stage1-5" => 5,
            "Stage1-6" => 6,
            "Stage1-7" => 7,
            "Stage1-8" => 8,
            "Stage1-9" => 9,
            "Stage1-10" => 10,
            _ => 1 // 기본값
        };

    }

    private static void SaveStageCount()
    {
        PlayerPrefs.SetInt("stageCount", stageCount);
        PlayerPrefs.Save();
    }

    public static void LoadStageCount()
    {
        if (PlayerPrefs.HasKey("stageCount"))
        {
            stageCount = PlayerPrefs.GetInt("stageCount");
        }
    }

    public static void SaveMaxUnlockedStage()
    {
        int currentMaxStage = PlayerPrefs.GetInt("MaxUnlockedStage", 1);
        if (stageCount > currentMaxStage)
        {
            PlayerPrefs.SetInt("MaxUnlockedStage", stageCount);
            PlayerPrefs.Save();
        }
    }

    public static int LoadMaxUnlockedStage()
    {
        return PlayerPrefs.GetInt("MaxUnlockedStage", 1);
    }

    public static string PreviousStage()
    {
        stageCount--;
        if (stageCount < 1) stageCount = 1;
        SaveStageCount();
        return CurrentStage();
    }

    public static int GetStageWinScore(string stageName)
    {
        if (stageWinScores.ContainsKey(stageName))
        {
            return stageWinScores[stageName];
        }
        return 0;
    }

}
