using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Chest : MonoBehaviour
{
    public List<Item> items;
    public SpriteRenderer image;
    public Sprite imageOpen, imageClose;
    private bool isOpen = false;
    public static Chest instance;
    private PlayerPrefs playerPrefs;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        GameObject playerPrefsObject = GameObject.FindWithTag("PlayerPrefs");
        if (playerPrefsObject != null)
            playerPrefs = playerPrefsObject.GetComponent<PlayerPrefs>();

        GameData.PickedUpItem chest = playerPrefs.locations[playerPrefs.location].PickedUpItems.Find(x =>
        x.locationOfPickingUp == playerPrefs.location &&
        x.sceneOfPickingUp == playerPrefs.scene &&
        x.nameInScene == transform.name);
        int indexItem = playerPrefs.locations[playerPrefs.location].PickedUpItems.IndexOf(chest);
        if (indexItem != -1 && items.Count>0)
        {
            items.Clear();
        }
    }

    public void Interaction()
    {
        if (isOpen)
        {
            isOpen = Close();
        }
        else
        {
            isOpen = Open();
        }

    }

    private bool Open()
    {
        image.sprite = imageOpen;
        DropAllItem();

        return true;
    }

    private bool Close()
    {
        image.sprite = imageClose;
        return false;
    }

    void DropAllItem()
    {
        if (items.Count == 0)
            return;
        foreach (Item item in items)
        {
            Vector3 spawnPosition = transform.position;
            Quaternion spawnRotation = Quaternion.identity;

            GameObject itemPrefab = Resources.Load<GameObject>("ItemsObjects/" + item.name);
            if (itemPrefab == null)
                continue;

            itemPrefab.GetComponent<ItemObject>().item = item;
            Instantiate(itemPrefab, spawnPosition, spawnRotation);
        }
        items.Clear();

        GameData.PickedUpItem chest = playerPrefs.locations[playerPrefs.location].PickedUpItems.Find(x => 
        x.locationOfPickingUp == playerPrefs.location &&
        x.sceneOfPickingUp == playerPrefs.scene && 
        x.nameInScene == transform.name);

        int indexItem = playerPrefs.locations[playerPrefs.location].PickedUpItems.IndexOf(chest);
        if (indexItem == -1)
        {
            playerPrefs.pickedUpItems++;
            int newId = playerPrefs.pickedUpItems;
            playerPrefs.locations[playerPrefs.location].PickedUpItems.Add(new GameData.PickedUpItem()
            {
                id = newId,
                locationOfPickingUp = playerPrefs.location,
                sceneOfPickingUp = playerPrefs.scene,
                name = "Chest",
                nameInScene = transform.name,
                lastPos = transform.position,
                selectedTime = playerPrefs.currentTime
            });
        }
    }
}
