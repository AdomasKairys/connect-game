using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    public UnityEvent OnGameEnd;

    [SerializeField] private GameObject buttonSpawnerPrefab;
    [SerializeField] private float minCameraSize = 6f;
    [SerializeField] private float maxCameraSize = 20f;


    private int _totalButtonCount;
    private int _currentButtonCount = 0;
    public static GameManager Instance { get; private set; }
    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(this.gameObject);
        else
            Instance = this;

        _totalButtonCount = LevelManager.Instance.GetCurrentLevelData().Length / 2;

        if (_totalButtonCount > 36)
            Debug.LogWarning("Total ammount of point is too large and could cause overlapping");

        // Scale the camera by the number of points
        Camera.main.orthographicSize = Mathf.Clamp(_totalButtonCount,minCameraSize, maxCameraSize);

        // Create button spawner
        Instantiate(buttonSpawnerPrefab);
    }

    public void IncrementButtonCount()
    { 
        _currentButtonCount++; 
        if(_currentButtonCount >= _totalButtonCount)
        {
            // Game ends
            OnGameEnd.Invoke();
        }
    }

}
