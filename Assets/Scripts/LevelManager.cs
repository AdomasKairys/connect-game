using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;


[System.Serializable]
public class Levels
{
    public LevelData[] levels;
}

[System.Serializable]
public class LevelData
{
    public string[] level_data;
}

public class LevelManager : MonoBehaviour
{
    [HideInInspector]
    public int CurrentLevelIndex = -1;
    [SerializeField]
    private TextAsset levelData;

    public static LevelManager Instance { get; private set; }
    public Levels Levels { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(this.gameObject);
        else
            Instance = this;
        DontDestroyOnLoad(gameObject);

        Levels = LoadLevelsFromFile();
    }
    private Levels LoadLevelsFromFile()
    {
        Assert.IsNotNull(levelData);

        Levels levelsInJson = JsonUtility.FromJson<Levels>(levelData.text);

        return levelsInJson;
    }
}