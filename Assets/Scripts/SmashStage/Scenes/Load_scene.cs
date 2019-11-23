using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Load_scene : MonoBehaviour
{
    public SceneReference scene;
    public static Load_scene instance;
    private void Awake()
    {
        instance = this;
    }
    //this script is very hard to understand because it loads a scene
    public void internal_loadscene()
    {
        SceneManager.LoadScene(scene.ScenePath, LoadSceneMode.Single);
    }
}
