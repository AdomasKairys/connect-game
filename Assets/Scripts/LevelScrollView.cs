using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelScrollView : MonoBehaviour
{
    [SerializeField] private GameObject levelItemPrefab;
    [SerializeField] private Transform levelScrollViewTransform;

    // Start is called before the first frame update
    void Start()
    {
        var levels = LevelManager.Instance.Levels.levels;
        for (int i = 0; i < levels.Length; i++)
        {
            GameObject levelItem = Instantiate(levelItemPrefab, levelScrollViewTransform);
            if(levelItem.TryGetComponent(out LevelItem item))
            {
                item.CreateLevel($"Level {i+1}", i);
            }
        }
    }
}
