using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuitButton : MonoBehaviour
{
    // Best would be to move it to a manager script
    public void QuitGame()
    {
        Application.Quit();
    }
}
