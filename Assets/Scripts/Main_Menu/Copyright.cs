using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Copyright : MonoBehaviour
{
    public Button button;
    [TextArea(15, 20), Tooltip("use %version% for version location")]
    public string copyrights;
    public TextMeshProUGUI text;
    // Start is called before the first frame update
    void Start()
    {
        text.text = copyrights.Replace("%version%", Application.version);
    }
    void Update()
    {
        if (Input.anyKey)
        {
            Buttonpress();
        }
    }
    public void Buttonpress()
    {
        button.onClick.Invoke();
    }
}
