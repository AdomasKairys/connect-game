using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelScrollView : MonoBehaviour
{
    [SerializeField] GameObject levelItemPrefab;
    [SerializeField] Transform levelScrollViewTransform;

    // Start is called before the first frame update
    void Start()
    {
        var levels = LevelManager.Instance.Levels.levels;
        for (int i = 0; i < levels.Length; i++)
        {
            GameObject levelItem = Instantiate(levelItemPrefab, levelScrollViewTransform);
            if(levelItem.TryGetComponent<LevelItem>(out LevelItem item))
            {
                item.CreateLevel("Level " + i, i);
            }
        }
    }
}
