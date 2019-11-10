using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AutomaticSlotSelectionMSS : MonoBehaviour {

	void Start () {

        Vector2 artworkOriginalSize = transform.Find("artwork").GetComponent<RectTransform>().sizeDelta;

        int random = Random.Range(0, SmashMSS.instance.maps.Count - 1);

        Map randomChar = SmashMSS.instance.maps[random];

        SmashMSS.instance.ShowMapInSlot(transform.GetSiblingIndex(), randomChar);

        transform.Find("artwork").GetComponent<RectTransform>().sizeDelta = artworkOriginalSize;
        transform.Find("artwork").GetComponent<RectTransform>().sizeDelta *= randomChar.zoom;
    }
	
}
