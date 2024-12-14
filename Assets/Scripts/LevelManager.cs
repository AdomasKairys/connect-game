using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.SceneManagement;


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
    [SerializeField] private string gameScene;
    [SerializeField] private string mainMenuScene;
    [SerializeField] private TextAsset levelData;
    private int _currentLevelIndex = -1;
    public static LevelManager Instance { get; private set; }  
    public Levels Levels { get; private set; }
    public string[] GetCurrentLevelData()
    {
        if (_currentLevelIndex == -1)
        {
            Debug.LogError("Current level is not selected", this);
            return null;
        }
        return Levels.levels[_currentLevelIndex].level_data;
    }
    public void LoadGameLevel(int levelIndex)
    {
        _currentLevelIndex = levelIndex;
        SceneManager.LoadSceneAsync(gameScene);
    }
    public void LoadMainMenu() => SceneManager.LoadSceneAsync(mainMenuScene);
    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(this.gameObject);
        else
            Instance = this;

        Levels = ReadLevelsFromFile();

        DontDestroyOnLoad(gameObject);
    }
    private Levels ReadLevelsFromFile()
    {
        Assert.IsNotNull(levelData);

        Levels levelsInJson = JsonUtility.FromJson<Levels>(levelData.text);

        return levelsInJson;
    }
}