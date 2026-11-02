using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameSession
{
    private const string UnlockedKey = "UnlockedLevels";
    public static int CurrentIndex { get; private set; }
    public static int UnlockedCount => PlayerPrefs.GetInt(UnlockedKey, 1);
    public static void SelectLevel(int index)
    {
        CurrentIndex = index;
    }
    public static void CompleteCurrentLevel()
    {
        int newCount = Mathf.Max(UnlockedCount, CurrentIndex + 2);
        PlayerPrefs.SetInt(UnlockedKey, newCount);
        PlayerPrefs.Save();
    }
}
