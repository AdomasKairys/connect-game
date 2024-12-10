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
    [SerializeField] TextAsset levelData;

    private static LevelManager _instance;
    public static LevelManager Instance { get { return _instance; } }

    private Levels _levels;
    public Levels Levels { get { return _levels; } }

    private int _currentLevelIndex = -1;
    public int CurrentLevelIndex { get { return _currentLevelIndex; } set { _currentLevelIndex = value; } }

    private void Awake()
    {
        if (_instance != null && _instance != this)
            Destroy(this.gameObject);
        else
            _instance = this;
        DontDestroyOnLoad(gameObject);

        _levels = LoadLevelsFromFile();
    }
    private Levels LoadLevelsFromFile()
    {
        Assert.IsNotNull(levelData);

        Levels levelsInJson = JsonUtility.FromJson<Levels>(levelData.text);

        return levelsInJson;
    }
}