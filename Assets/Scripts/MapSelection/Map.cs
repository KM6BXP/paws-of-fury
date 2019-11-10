using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "New Map", menuName = "Map")]
public class Map : ScriptableObject {

    public string mapName;
    public Sprite mapSprite;
    public Sprite mapIcon;
    public float zoom = 0.39F;

    public Color backgroundColor;
}
