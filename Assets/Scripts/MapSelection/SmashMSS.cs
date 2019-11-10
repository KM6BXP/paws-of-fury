using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class SmashMSS : MonoBehaviour
{

    private GridLayoutGroup gridLayout;
    private RectTransform gridSize;

    private float rowSize;
    private float rowCount;
    private float ogWidth;
    private float ogHeight;

    private List<GameObject> mapObjects;
    [HideInInspector]
    public Vector2 slotArtworkSize;


    public static SmashMSS instance;

    //debug clears the map grid and readds them every frame (only use when fine tuning map sprites, this causes a gicantic fps drop)
    public bool debug = false;
    [Header("Maps List")]
    public List<Map> maps = new List<Map>();
    [Space]
    [Header("Public References")]
    public GameObject charCellPrefab;
    public GameObject gridBgPrefab;
    public Transform playerSlotsContainer;
    [Space]
    [Header("Current Confirmed Map")]
    public Map confirmedMap;

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
            Destroy(child.gameObject);
        }
        SpawnMaps();
    }

    //get components and spawn maps
    void Start()
    {
        gridLayout = GetComponent<GridLayoutGroup>();
        gridSize = GetComponent<RectTransform>();
        gridSize.sizeDelta = new Vector2(gridLayout.cellSize.x * 5, gridLayout.cellSize.y * 2);
        slotArtworkSize = playerSlotsContainer.GetChild(0).Find("artwork").GetComponent<RectTransform>().sizeDelta;


        ogWidth = gridLayout.cellSize.x;
        ogHeight = gridLayout.cellSize.y;

        SpawnMaps();
    }

    //this was appearently so hard to do it took a week to crack this
    private void SpawnMaps()
    {
        int spawnedMaps = 0;
        //default grid size
        rowSize = 4;
        rowCount = 1;

        //the magic, don't touch this plz
        foreach (Map map in maps)
        {
            SpawnMapCell(map);
            spawnedMaps++;
            if (rowSize * rowCount < spawnedMaps)
            {
                if (rowCount - 1 <= Math.Round(rowSize * gridSize.sizeDelta.y / gridSize.sizeDelta.x) && Math.Round(rowSize * gridSize.sizeDelta.y / gridSize.sizeDelta.x) <= rowCount)
                    rowSize++;
                rowCount = (float)Math.Ceiling(spawnedMaps / rowSize);
                gridLayout.cellSize = new Vector2(gridSize.sizeDelta.x / rowSize, gridSize.sizeDelta.x / rowSize * ogHeight / ogWidth);
            }
        }
        foreach (GameObject map in mapObjects)
        {
            map.GetComponent<RectTransform>().sizeDelta *= ogWidth / gridLayout.cellSize.x;
        }
    }

    //create map slot with sprite
    private void SpawnMapCell(Map map)
    {
        GameObject charCell = Instantiate(charCellPrefab, transform);

        charCell.name = map.mapName;

        Image background = charCell.transform.Find("background").GetComponent<Image>();
        Image artwork = charCell.transform.Find("artwork").GetComponent<Image>();
        TextMeshProUGUI name = charCell.transform.Find("nameRect").GetComponentInChildren<TextMeshProUGUI>();

        background.color = map.backgroundColor;
        artwork.sprite = map.mapSprite;
        name.text = map.mapName;

        artwork.GetComponent<RectTransform>().pivot = uiPivot(artwork.sprite);
        artwork.GetComponent<RectTransform>().sizeDelta *= map.zoom;
    }

    //show map in slot
    public void ShowMapInSlot(int player, Map map)
    {
        bool nullChar = (map == null);

        Color alpha = nullChar ? Color.clear : Color.white;
        Sprite artwork = nullChar ? null : map.mapSprite;
        string name = nullChar ? string.Empty : map.mapName;
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
            slotIcon.GetComponent<Image>().sprite = map.mapIcon;
            slotIcon.GetComponent<Image>().DOFade(.3f, 0);
        }

        if (artwork != null)
        {
            slotArtwork.GetComponent<RectTransform>().pivot = uiPivot(artwork);
            slotArtwork.GetComponent<RectTransform>().sizeDelta = slotArtworkSize;
            slotArtwork.GetComponent<RectTransform>().sizeDelta *= map.zoom;
        }
        slot.Find("name").GetComponent<TextMeshProUGUI>().text = name;
        slot.Find("player").GetComponentInChildren<TextMeshProUGUI>().text = playernickname;
        slot.Find("iconAndPx").GetComponentInChildren<TextMeshProUGUI>().text = playernumber;
    }

    //make it shake when selected
    public void ConfirmMap(int player, Map map)
    {
        if (confirmedMap == null)
        {
            confirmedMap = map;
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
