using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteScaler : MonoBehaviour
{
    [SerializeField] SpriteRenderer spriteRenderer;

    private void Start()
    {
        Vector2 spriteSize = spriteRenderer.sprite.bounds.size;

        // Get the size of the camera in world space
        Camera camera = Camera.main;
        float screenAspect = (float)Screen.width / Screen.height;
        float cameraHeight = camera.orthographicSize * 2;
        float cameraWidth = cameraHeight * screenAspect;

        // Calculate the scale to fit the sprite to the camera
        Vector3 newScale = transform.localScale;
        newScale.x = cameraWidth / spriteSize.x;
        newScale.y = cameraHeight / spriteSize.y;

        float scaleFactor = Mathf.Max(newScale.x, newScale.y);
        spriteRenderer.transform.localScale = new Vector3(scaleFactor, scaleFactor, 1);
    }
}
