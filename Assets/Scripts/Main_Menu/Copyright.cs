using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Copyright : MonoBehaviour
{

    [TextArea(15,20), Tooltip("use %version% for version location")]
    public string copyrights;
    public TextMeshProUGUI text;
    // Start is called before the first frame update
    void Start()
    {
        text.text = copyrights.Replace("%version%", Application.version);
    }
}
