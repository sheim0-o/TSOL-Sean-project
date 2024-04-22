using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour, IDropHandler
{
    public Image image;
    public Sprite selectSprite, deselectSprite;

    private void Awake()
    {
        Deselect();
    }

    public void OnDrop(PointerEventData eventData)
    {
        InventoryItemSlot itemSlot = GetComponentInChildren<InventoryItemSlot>();
        InventoryItemSlot itemSlotDrag = eventData.pointerDrag.GetComponent<InventoryItemSlot>();
        if (itemSlot != null &&
               itemSlot.item == itemSlotDrag.item &&
               itemSlot.count < InventorySystem.instance.maxStackedItems &&
               itemSlot.item.stackable)
        {
            itemSlot.count += itemSlotDrag.count;
            itemSlot.RefreshCount();
            itemSlot.parentAfterDrag = transform;
            Destroy(itemSlotDrag.transform.gameObject);
            return;
        }
        if (transform.childCount == 0)
        {
            InventoryItemSlot inventoryItemSlot = eventData.pointerDrag.GetComponent<InventoryItemSlot>();
            inventoryItemSlot.parentAfterDrag = transform;
        }
    }

    public void Select()
    {
        image.sprite = selectSprite;
    }
    public void Deselect()
    {
        image.sprite = deselectSprite;
    }


}
