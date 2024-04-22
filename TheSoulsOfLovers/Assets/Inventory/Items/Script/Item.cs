using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;


[CreateAssetMenu(fileName = "New Item", menuName = "Inventory System/Items/New Item")]
public class Item : ScriptableObject
{
    [Header("Gameplay")]
    public TileBase tile;
    public ItemType itemType;
    public ActionType actionType;
    public float valueOfTheItemAction = 1;
    public Vector2Int range = new Vector2Int(5, 4);

    [Header("UI")]
    public bool stackable = true;

    [Header("Both")]
    public Sprite image;


    [HideInInspector] public string nameInScene = "";
    [HideInInspector] public int id = 0;
}

public enum ItemType
{
    None,
    Tool,
    Weapon,
    Food,
    Quest,
    Amulet
}   

public enum ActionType
{
    None,
    Beat,
    Eat,
    Open
}
