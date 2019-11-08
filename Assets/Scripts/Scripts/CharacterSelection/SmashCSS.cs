using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class SmashCSS : MonoBehaviour
{

    private GridLayoutGroup gridLayout;
    private RectTransform gridSize;

    private float rowSize;
    private float rowCount;
    private float ogWidth;
    private float ogHeight;

    private List<GameObject> characterObjects;
    [HideInInspector]
    public Vector2 slotArtworkSize;


    public static SmashCSS instance;

    //debug clears the character grid and readds them every frame (only use when fine tuning character sprites, this causes a gicantic fps drop)
    public bool debug = false;
    [Header("Characters List")]
    public List<Character> characters = new List<Character>();
    [Space]
    [Header("Public References")]
    public GameObject charCellPrefab;
    public GameObject gridBgPrefab;
    public Transform playerSlotsContainer;
    [Space]
    [Header("Current Confirmed Character")]
    public Character confirmedCharacter;

    private void Awake()
    {
        instance = this;
    }

    private void Update()
    {
        //the debug feature
        if (!debug)
            return;
        foreach (Transform child in gridLayout.transform)
        {
            GameObject.Destroy(child.gameObject);
        }
        SpawnCharacters();
    }

    //get components and spawn characters
    void Start()
    {
        gridLayout = GetComponent<GridLayoutGroup>();
        gridSize = GetComponent<RectTransform>();
        gridSize.sizeDelta = new Vector2(gridLayout.cellSize.x * 5, gridLayout.cellSize.y * 2);
        slotArtworkSize = playerSlotsContainer.GetChild(0).Find("artwork").GetComponent<RectTransform>().sizeDelta;


        ogWidth = gridLayout.cellSize.x;
        ogHeight = gridLayout.cellSize.y;

        SpawnCharacters();
    }

    //this was appearently so hard to do it took a week to crack this
    private void SpawnCharacters()
    {
        int spawnedCharacters = 0;
        //default grid size
        rowSize = 4;
        rowCount = 1;

        //the magic, don't touch this plz
        foreach (Character character in characters)
        {
            SpawnCharacterCell(character);
            spawnedCharacters++;
            if (rowSize * rowCount < spawnedCharacters)
            {
                if (rowCount - 1 <= Math.Round(rowSize * gridSize.sizeDelta.y / gridSize.sizeDelta.x) && Math.Round(rowSize * gridSize.sizeDelta.y / gridSize.sizeDelta.x) <= rowCount)
                    rowSize++;
                rowCount = (float)Math.Ceiling(spawnedCharacters / rowSize);
                gridLayout.cellSize = new Vector2(gridSize.sizeDelta.x / rowSize, gridSize.sizeDelta.x / rowSize * ogHeight / ogWidth);
            }
        }
        foreach (GameObject character in characterObjects)
        {
            character.GetComponent<RectTransform>().sizeDelta *= ogWidth / gridLayout.cellSize.x;
        }
    }

    //create character slot with sprite
    private void SpawnCharacterCell(Character character)
    {
        GameObject charCell = Instantiate(charCellPrefab, transform);

        charCell.name = character.characterName;

        Image background = charCell.transform.Find("background").GetComponent<Image>();
        Image artwork = charCell.transform.Find("artwork").GetComponent<Image>();
        TextMeshProUGUI name = charCell.transform.Find("nameRect").GetComponentInChildren<TextMeshProUGUI>();

        background.color = character.backgroundColor;
        artwork.sprite = character.characterSprite;
        name.text = character.characterName;

        artwork.GetComponent<RectTransform>().pivot = uiPivot(artwork.sprite);
        artwork.GetComponent<RectTransform>().sizeDelta *= character.zoom;
    }

    //show character in slot
    public void ShowCharacterInSlot(int player, Character character)
    {
        bool nullChar = (character == null);

        Color alpha = nullChar ? Color.clear : Color.white;
        Sprite artwork = nullChar ? null : character.characterSprite;
        string name = nullChar ? string.Empty : character.characterName;
        string playernickname = "Player " + (player + 1).ToString();
        string playernumber = "P" + (player + 1).ToString();

        Transform slot = playerSlotsContainer.GetChild(player);

        Transform slotArtwork = slot.Find("artwork");
        Transform slotIcon = slot.Find("icon");

        Sequence s = DOTween.Sequence();
        s.Append(slotArtwork.DOLocalMoveX(-300, .1f).SetEase(Ease.OutCubic));
        s.AppendCallback(() => slotArtwork.GetComponent<Image>().sprite = artwork);
        s.AppendCallback(() => slotArtwork.GetComponent<Image>().color = alpha);
        s.Append(slotArtwork.DOLocalMoveX(300, 0));
        s.Append(slotArtwork.DOLocalMoveX(0, .1f).SetEase(Ease.OutCubic));

        if (nullChar)
        {
            slotIcon.GetComponent<Image>().DOFade(0, 0);
        }
        else
        {
            slotIcon.GetComponent<Image>().sprite = character.characterIcon;
            slotIcon.GetComponent<Image>().DOFade(.3f, 0);
        }

        if (artwork != null)
        {
            slotArtwork.GetComponent<RectTransform>().pivot = uiPivot(artwork);
            slotArtwork.GetComponent<RectTransform>().sizeDelta = slotArtworkSize;
            slotArtwork.GetComponent<RectTransform>().sizeDelta *= character.zoom;
        }
        slot.Find("name").GetComponent<TextMeshProUGUI>().text = name;
        slot.Find("player").GetComponentInChildren<TextMeshProUGUI>().text = playernickname;
        slot.Find("iconAndPx").GetComponentInChildren<TextMeshProUGUI>().text = playernumber;
    }

    //make it shake when selected
    public void ConfirmCharacter(int player, Character character)
    {
        if (confirmedCharacter == null)
        {
            confirmedCharacter = character;
            playerSlotsContainer.GetChild(player).DOComplete();
            playerSlotsContainer.GetChild(player).DOPunchPosition(Vector3.down * 3, .3f, 10, 1);
            
        }
    }

    //set sprite center
    public Vector2 uiPivot(Sprite sprite)
    {
        Vector2 pixelSize = new Vector2(sprite.texture.width, sprite.texture.height);
        Vector2 pixelPivot = sprite.pivot;
        return new Vector2(pixelPivot.x / pixelSize.x, pixelPivot.y / pixelSize.y);
    }

}
