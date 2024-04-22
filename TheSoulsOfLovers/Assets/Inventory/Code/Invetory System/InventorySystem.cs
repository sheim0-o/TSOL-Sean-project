using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;



public class InventorySystem : MonoBehaviour
{
    public int maxStackedItems = 64;
    public InventorySlot[] inventorySlots;
    public GameObject invetoryItemPrebaf;
    public static InventorySystem instance;

    public int position = 0;

    public void SelectNextItem()
    {
        if (position != inventorySlots.Length - 1)
            ChangeSelectedSlot(position + 1);
        else
            ChangeSelectedSlot(0);
    }
    public void SelectPreviousItem()
    {
        if (position != 0)
            ChangeSelectedSlot(position - 1);
        else
            ChangeSelectedSlot(inventorySlots.Length - 1);
    }

    public void Update()
    {
        instance = this;
    }

    public void Start()
    {
        inventorySlots[position].Select();
    }

    public Item GetSelectItem()
    {
        if (inventorySlots[position].GetComponentInChildren<InventoryItemSlot>() != null)
        {
            InventoryItemSlot selectItem = inventorySlots[position].GetComponentInChildren<InventoryItemSlot>();
            return selectItem.item;
        }
        Item itemNull = null;
        return itemNull;
    }
    
    public void ChangeSelectedSlot(int selectValue)
    {
        inventorySlots[position].Deselect();
        inventorySlots[selectValue].Select();
        position = selectValue;
    }

    public bool AddItem(Item item)
    {
        for (int i = 0; i < inventorySlots.Length; i++)
        {
            InventorySlot slot = inventorySlots[i];
            InventoryItemSlot itemSlot = slot.GetComponentInChildren<InventoryItemSlot>();
            if (itemSlot != null &&
                itemSlot.item == item &&
                itemSlot.count < maxStackedItems &&
                itemSlot.item.stackable)
            {
                itemSlot.count++;
                itemSlot.RefreshCount();
                return true;
            }
        }



        for (int i=0; i<inventorySlots.Length; i++)
        {
            InventorySlot slot = inventorySlots[i];
            InventoryItemSlot itemSlot = slot.GetComponentInChildren<InventoryItemSlot>();
            if(itemSlot == null)
            {
                SpawnItem(item, slot);
                return true;
            }
        }

        return false;

    }

    public bool DropItem(Item item)
    {
        InventorySlot slot = inventorySlots[position];
        InventoryItemSlot itemSlot = slot.GetComponentInChildren<InventoryItemSlot>();
        if (itemSlot != null &&
            itemSlot.item == item &&
            itemSlot.count < maxStackedItems &&
            item != null)
        {
            if (itemSlot.item.stackable)
            {
                if(itemSlot.count == 1)
                {
                    DeleteItem(slot);
                }
                itemSlot.count--;
                itemSlot.RefreshCount();
            }
            else
            {
                DeleteItem(slot);
            }
            return true;
        }

        return false;

    }

    public void DeleteItem(InventorySlot slot)
    {
        InventoryItemSlot itemSlotDrag = slot.GetComponentInChildren<InventoryItemSlot>();
        Destroy(itemSlotDrag.transform.gameObject);
    }

    void SpawnItem(Item item, InventorySlot slot)
    {
        GameObject itemGameObject = Instantiate(invetoryItemPrebaf, slot.transform);
        InventoryItemSlot inventoryItemSlot = itemGameObject.GetComponent<InventoryItemSlot>();
        inventoryItemSlot.InitItem(item);
    }
}
