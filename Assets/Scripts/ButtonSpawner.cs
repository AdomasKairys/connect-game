using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
    }

    private void SpawnButtons()
    {
        var coordinateToSpawn = LevelDataToCoordinates();

        GameObject currentObject = null;
        GameObject firstObject = null;
        GameObject previousObject = null;
        for (int i = 0; i < coordinateToSpawn.Length; i++, previousObject = currentObject)
        {
            currentObject = Instantiate(buttonPrefab, coordinateToSpawn[i], Quaternion.identity);
            bool hasComponent = currentObject.TryGetComponent(out ButtonController buttonController);

            if (!hasComponent)
                continue;

            buttonController.SetNumberText((i+1).ToString());

            if (previousObject == null)
            {
                firstObject = currentObject;
                buttonController.isRoot = true;
                continue;
            }

            if (previousObject.TryGetComponent(out ButtonController previousButtonController))
                buttonController.previousButton = previousButtonController;
        }
        if (firstObject != null && currentObject != null)
        {
            firstObject.GetComponent<ButtonController>().previousButton = currentObject.GetComponent<ButtonController>();
        }
    }

    private Vector3[] LevelDataToCoordinates()
    {
        string[] coordinates = LevelManager.Instance.Levels.levels[LevelManager.Instance.CurrentLevelIndex].level_data;

        Vector3[] coords = coordinates.Where((_,i)=> i % 2 == 0)
            .Zip(
                coordinates.Where((_, i) => (i+1) % 2 == 0),
                (x, y) => new Vector3(1-float.Parse(x)/1000, 1-float.Parse(y)/1000, -_mainCamera.transform.position.z)
            )
            .Select((v) => _mainCamera.ViewportToWorldPoint(v))
            .ToArray();

        return coords;
    }
}
