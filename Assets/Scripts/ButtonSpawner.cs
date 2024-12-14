using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using UnityEngine;

public class ButtonSpawner : MonoBehaviour
{
    [SerializeField] private GameObject buttonPrefab;
    private Camera _mainCamera;
    private void Awake()
    {
        _mainCamera = Camera.main;
    }
    void Start()
    {
        SpawnButtons();
        Destroy(gameObject);
    }
    private void SpawnButtons()
    {
        var coordinates = LevelDataAsCoordinates();
        ButtonController previousButton = null;
        ButtonController rootButton = null;
        for (int i = 0; i < coordinates.Length; i++)
        {
            // Instantiate button
            var buttonObject = Instantiate(buttonPrefab, coordinates[i], Quaternion.identity);

            // Configure button
            if (!buttonObject.TryGetComponent(out ButtonController buttonController))
                continue;

            buttonController.buttonIndex = i;
            buttonController.previousButton = previousButton;

            // Mark the first button as the root
            if (i == 0)
            {
                buttonController.isRoot = true;
                rootButton = buttonController;
            }

            previousButton = buttonController;
        }

        // Close the circular link
        if (rootButton != null && previousButton != null)
            rootButton.previousButton = previousButton;
    }
    private Vector3[] LevelDataAsCoordinates()
    {
        var _levelData = LevelManager.Instance.GetCurrentLevelData();

        if (_levelData.Length % 2 != 0)
            Debug.LogWarning("Level data list doens't contain an even number of elements, coordinates will be missing", this);

        Vector3[] coordinates = _levelData.Where((_,i)=> i % 2 == 0)
            .Zip(
                _levelData.Where((_, i) => (i+1) % 2 == 0),
                (x, y) => new Vector3(1-float.Parse(x)/1000, 1-float.Parse(y)/1000, -_mainCamera.transform.position.z)
            )
            .Select((v) => _mainCamera.ViewportToWorldPoint(v))
            .ToArray();

        return coordinates;
    }
}
