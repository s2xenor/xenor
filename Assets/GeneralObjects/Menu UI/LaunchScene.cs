using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LaunchScene : MonoBehaviour
{
    public int sceneInt;
    public string sceneString;
    // Only one is needed

    public void LoadScene()
    {
        if (sceneString != null && sceneString != "")
            SceneManager.LoadScene(sceneString); // Scene to load
        else
            SceneManager.LoadScene(sceneInt); // Scene to load
    }
}
