using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine.SceneManagement;

public static class Stage // 어떤 역할?
{
    public static int stageCount { get; private set; } = 1;
    public static string CurrentStage()
    {
        string result = stageCount switch
        {
            1 => "Stage1-1",
            2 => "Stage1-2",
            3 => "Stage1-3",
            4 => "Stage1-4",
            _ => "Stage1-1" // 기본값
        };

        return result;
    }
    
    public static string NextStage()
    {
        stageCount++;

        return CurrentStage();
    }

    public static void SetStage(string str)
    {
        stageCount = str switch
        {
            "Stage1-1" => 1,
            "Stage1-2" => 2,
            "Stage1-3" => 3,
            "Stage1-4" => 4,
            _ => 1 // 기본값
        };

    }
    

    //public static string currentStage { get; set; } = "Stage1-1";


    //public static Dictionary<string, int> ht = new Dictionary<string, int>();

    //public static void Init()
    //{
    //    // 추후 csv 매칭역할
    //}

    //public static int NextStage()
    //{


    //    switch


    //    return ht[currentStage];
    //}





}
