using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelItem : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI levelText;
    [SerializeField] Button levelButton;
    private int _levelDataIndex;

    public void CreateLevel(string levelName, int levelDataIndex)
    {
        levelText.text = levelName;
        _levelDataIndex = levelDataIndex;
        levelButton.onClick.AddListener(() => EnterLevel());
    }

    private void EnterLevel()
    {
        LevelManager.Instance.CurrentLevelIndex = _levelDataIndex;

        SceneManager.LoadScene(LevelManager.Instance.CurrentLevelIndex); //switch to async later
    }
}
