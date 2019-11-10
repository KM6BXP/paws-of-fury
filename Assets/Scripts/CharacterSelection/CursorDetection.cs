using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using DG.Tweening;

public class CursorDetection : MonoBehaviour
{

    private GraphicRaycaster gr;
    private PointerEventData pointerEventData = new PointerEventData(null);

    private float timer;

    public Transform currentCharacter;
    public Button confirm;
    public Transform token;
    public bool hasToken;
    public bool existsToken;

    void Start()
    {

        gr = GetComponentInParent<GraphicRaycaster>();
        SmashCSS.instance.ShowCharacterInSlot(0, null);
        confirm.gameObject.SetActive(false);
    }

    void Update()
    {

        //send a ray to see what's under the cursor
        pointerEventData.position = Camera.main.WorldToScreenPoint(transform.position);
        List<RaycastResult> results = new List<RaycastResult>();
        gr.Raycast(pointerEventData, results);
        Debug.Log(results);
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
            if (currentCharacter != null)
            {
                TokenFollow(false);
                SmashCSS.instance.ConfirmCharacter(0, SmashCSS.instance.characters[currentCharacter.GetSiblingIndex()]);
            }
            timer = 0f;
        }

        //CANCEL
        if (Input.GetKeyDown(KeyCode.Space) && !hasToken && existsToken && timer >= .5f)
        {
            SmashCSS.instance.confirmedCharacter = null;
            TokenFollow(true);
            timer = 0f;
        }

        if (hasToken)
        {
            token.position = transform.position;
        }

        //checks what character is selected i think
        if (hasToken)
        {
            if (results.Count > 0 && results[0].gameObject.tag == "Selectable")
            {
                Transform raycastCharacter = results[0].gameObject.transform;

                if (raycastCharacter != currentCharacter)
                {
                    if (currentCharacter != null)
                    {
                        currentCharacter.Find("selectedBorder").GetComponent<Image>().DOKill();
                        currentCharacter.Find("selectedBorder").GetComponent<Image>().color = Color.clear;
                    }
                    SetCurrentCharacter(raycastCharacter);
                }
            }
            else
            {
                if (currentCharacter != null)
                {
                    currentCharacter.Find("selectedBorder").GetComponent<Image>().DOKill();
                    currentCharacter.Find("selectedBorder").GetComponent<Image>().color = Color.clear;
                    SetCurrentCharacter(null);
                }
            }
        }

    }

    void SetCurrentCharacter(Transform t)
    {

        if (t != null)
        {
            t.Find("selectedBorder").GetComponent<Image>().color = Color.white;
            t.Find("selectedBorder").GetComponent<Image>().DOColor(Color.red, .7f).SetLoops(-1);
        }

        currentCharacter = t;

        if (t != null)
        {
            int index = t.GetSiblingIndex();
            Character character = SmashCSS.instance.characters[index];
            SmashCSS.instance.ShowCharacterInSlot(0, character);
        }
        else
        {
            SmashCSS.instance.ShowCharacterInSlot(0, null);
        }
    }

    void TokenFollow(bool trigger)
    {
        hasToken = trigger;
        confirm.gameObject.SetActive(!trigger);
    }
}
