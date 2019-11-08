using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Global : MonoBehaviour
{
    #region singleton
    //basic singleton pattern
    private static Global global;

    public static Global Instance { get { return global; } }

    void Awake()
    {
        if (global == null)
        {
            DontDestroyOnLoad(gameObject);
            global = this;
        }
        else if (global != this)
        {
            Destroy(gameObject);
        }
    }
    #endregion

    public Character[] PickedCharacters;

    public void Quit()
    {
        Application.Quit();
    }
}
