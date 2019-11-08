using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Load_scene : MonoBehaviour
{

    //this script is very hard to understands because it loads a scene
    public SceneReference scene;
    public void internal_loadscene()
    {
        SceneManager.LoadScene(scene.ScenePath, LoadSceneMode.Single);
    }
}
