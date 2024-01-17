using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    private const string LEVEL_KEY = "CurrentLevel";

    public static int GetCurrentLevel()
    {
        return PlayerPrefs.GetInt(LEVEL_KEY, 1);
    }

    public static void SetCurrentLevel(int level)
    {
        PlayerPrefs.SetInt(LEVEL_KEY, level);
    }
}
