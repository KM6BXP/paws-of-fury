using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using DG.Tweening;

public class CursorDetectionMSS : MonoBehaviour
{

    private GraphicRaycaster gr;
    private PointerEventData pointerEventData = new PointerEventData(null);

    private float timer;

    public Transform currentMap;
    public Button confirm;
    public Transform token;
    public bool hasToken;
    public bool existsToken;

    void Start()
    {

        gr = GetComponentInParent<GraphicRaycaster>();
        SmashMSS.instance.ShowMapInSlot(0, null);
        confirm.gameObject.SetActive(false);
    }

    void Update()
    {

        //send a ray to see what's under the cursor
        pointerEventData.position = Camera.main.WorldToScreenPoint(transform.position);
        List<RaycastResult> results = new List<RaycastResult>();
        gr.Raycast(pointerEventData, results);
        //checks if there's a button under the cursor to press
        if (results.Count > 0)
        {
            foreach (RaycastResult result in results)
            {
                if (result.gameObject.GetComponent<Button>() != null && Input.GetKeyDown(KeyCode.Space))
                {
                    Debug.Log("exit");
                    result.gameObject.GetComponent<Button>().onClick.Invoke();
                    timer = 0f;
                    return;
                }
            }
        }

        //this handles the token placement
        if (timer < .5f)
            timer += Time.deltaTime;
        //CONFIRM
        if (Input.GetKeyDown(KeyCode.Space) && hasToken && existsToken && timer >= .5f)
        {
            if (currentMap != null)
            {
                TokenFollow(false);
                SmashMSS.instance.ConfirmMap(0, SmashMSS.instance.maps[currentMap.GetSiblingIndex()]);
            }
            timer = 0f;
        }

        //CANCEL
        if (Input.GetKeyDown(KeyCode.Space) && !hasToken && existsToken && timer >= .5f)
        {
            SmashMSS.instance.confirmedMap = null;
            TokenFollow(true);
            timer = 0f;
        }

        if (hasToken)
        {
            token.position = transform.position;
        }

        //checks what map is selected i think
        if (hasToken)
        {
            if (results.Count > 0 && results[0].gameObject.tag == "Selectable")
            {
                Transform raycastMap = results[0].gameObject.transform;

                if (raycastMap != currentMap)
                {
                    if (currentMap != null)
                    {
                        currentMap.Find("selectedBorder").GetComponent<Image>().DOKill();
                        currentMap.Find("selectedBorder").GetComponent<Image>().color = Color.clear;
                    }
                    SetCurrentMap(raycastMap);
                }
            }
            else
            {
                if (currentMap != null)
                {
                    currentMap.Find("selectedBorder").GetComponent<Image>().DOKill();
                    currentMap.Find("selectedBorder").GetComponent<Image>().color = Color.clear;
                    SetCurrentMap(null);
                }
            }
        }

    }

    void SetCurrentMap(Transform t)
    {

        if (t != null)
        {
            t.Find("selectedBorder").GetComponent<Image>().color = Color.white;
            t.Find("selectedBorder").GetComponent<Image>().DOColor(Color.red, .7f).SetLoops(-1);
        }

        currentMap = t;

        if (t != null)
        {
            int index = t.GetSiblingIndex();
            Map map = SmashMSS.instance.maps[index];
            SmashMSS.instance.ShowMapInSlot(0, map);
        }
        else
        {
            SmashMSS.instance.ShowMapInSlot(0, null);
        }
    }

    void TokenFollow(bool trigger)
    {
        hasToken = trigger;
        confirm.gameObject.SetActive(!trigger);
    }
}
