using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemObject : MonoBehaviour
{
    public Item item;
    private void OnValidate()
    {
        GetComponentInChildren<SpriteRenderer>().sprite = item.image;
    }
    private void Awake()
    {
        GetComponentInChildren<SpriteRenderer>().sprite = item.image;
    }
    public ItemObject(Item item)
    {
        this.item = item;
    }

}
