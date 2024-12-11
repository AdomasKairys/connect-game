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
        var coordinateToSpawn = LevelDataToCoordinates();

        GameObject previousObject = null;
        GameObject currentObject;
        for (int i = 0; i < coordinateToSpawn.Length; i++, previousObject = currentObject)
        {
            currentObject = Instantiate(buttonPrefab, coordinateToSpawn[i], Quaternion.identity);
            bool hasComponent = currentObject.TryGetComponent(out ButtonController buttonController);

            if(!hasComponent)
                continue;

            if(previousObject == null)
            {
                buttonController.isReadyToRender = true;
                buttonController.isActiveToClick = true;
                continue;
            }

            if (previousObject.TryGetComponent(out ButtonController previousButtonController))
                previousButtonController.connectedButton = buttonController;
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
