using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelItem : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI levelText;

    private int _levelDataIndex;
    public void CreateLevel(string levelName, int levelDataIndex)
    {
        levelText.text = levelName;
        _levelDataIndex = levelDataIndex;
    }

    public void EnterGameLevel()
    {
        LevelManager.Instance.LoadGameLevel(_levelDataIndex);
    }
}
