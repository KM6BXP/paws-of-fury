using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Load_scene : MonoBehaviour
{
    public SceneReference scene;
    public void internal_loadscene()
    {
        SceneManager.LoadScene(scene.ScenePath, LoadSceneMode.Single);
    }
}
