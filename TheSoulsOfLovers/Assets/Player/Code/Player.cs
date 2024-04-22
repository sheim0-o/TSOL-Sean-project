using System;
using System.Collections;
using System.Linq;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static UnityEngine.GraphicsBuffer;
using TMPro;

public class Player : MonoBehaviour, IDataPersistence
{
    private Vector2 direction;
    private Vector2 rollDir;
    private Rigidbody2D rigidbody2D;
    private float dirX, dirY;
    private bool isAttacking = false;
    private bool activatedAmulet = false;
    private bool isRolling = false;
    private QuestHandler questHandler;
    protected Coroutine coroutine;
    private bool hasAmulet = false;
    private TextMeshProUGUI infoUIText;

    [HideInInspector] public Animator animator;
    [HideInInspector] public int health=5;
    [HideInInspector] public GameData gd;
    [HideInInspector] public bool lockPlayer = false;

    public float attackExitTime = 0.45F;
    public float rollingExitTime = 0.45F;
    public float rollingSpeed = 5F;
    
    public float speed = 3F;
    public int maxHealth = 5;
    public GameObject deathMenu;
    public HealthBar healthBar;
    public PlayerPrefs playerPrefs;

    //Inventory
    [HideInInspector] public ShowDialogue targetNPC;
    [HideInInspector] public ItemObject targetItem;
    [HideInInspector] public Chest targetChest;
    [HideInInspector] public Keyhole targetKeyHole;
    public InventorySystem inventory;

    void Start()
    {
        animator = GetComponent<Animator>();
        rigidbody2D = GetComponent<Rigidbody2D>();
        if(GameObject.FindWithTag("QuestHandler"))
            questHandler = GameObject.FindWithTag("QuestHandler").GetComponent<QuestHandler>();
        if (GameObject.FindWithTag("InteractableText"))
            infoUIText = GameObject.FindWithTag("InteractableText").GetComponent<TextMeshProUGUI>();
    }
    void Update()
    {
        if (lockPlayer)
        {
            animator.SetFloat("Speed", 0);
            return;
        }

        if (!isRolling)
            Direction();
        SetCondition();
            
    }
    void FixedUpdate()
    {
        if (direction.sqrMagnitude > 0 && health != 0 && 
            !isAttacking && !isRolling && !activatedAmulet && !lockPlayer)
        {
            MoveCharacter(speed);
        }
        else if (isRolling)
        {
            MoveCharacter(rollingSpeed);
            //Vector2 rollDir = new Vector2(dirX, dirY);
            //rigidbody2D.MovePosition(rigidbody2D.position + (rollDir * rollingSpeed * 4) * Time.deltaTime);
            return;
        }
    }

    public void SetCondition()
    {
        bool hasInventory = inventory != null;

        if (isAttacking || activatedAmulet)
            return;
        else if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            Rolling();
        }
        else if (Input.GetKeyDown(KeyCode.R) && hasAmulet)
        {
            ActivateAmulet();
        }
        else if(hasInventory)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                Interaction(hasInventory);
            }
            else if (Input.GetKeyDown(KeyCode.J))
            {
                if (inventory.GetSelectItem() != null)
                {
                    switch (inventory.GetSelectItem().itemType)
                    {
                        case ItemType.Weapon:
                            if (!isAttacking)
                                Attack();
                            break;
                        case ItemType.Food:
                            if (health != maxHealth)
                            {
                                SetHealth(health + (int)inventory.GetSelectItem().valueOfTheItemAction);
                                InventorySystem.instance.DropItem(inventory.GetSelectItem());
                            }
                            break;
                    }
                }
            }
            else if (Input.GetKeyDown(KeyCode.M))
            {
                DropItem();
            }
            else if (Input.GetKeyDown(KeyCode.K))
            {
                inventory.SelectPreviousItem();
            }
            else if (Input.GetKeyDown(KeyCode.L))
            {
                inventory.SelectNextItem();
            }
        }
    }


    public void Direction()
    {
        direction = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;

        if (direction != Vector2.zero)
        {
            dirX = (float)Math.Round(direction.x);
            dirY = (float)Math.Round(direction.y);
            animator.SetFloat("MoveX", dirX);
            animator.SetFloat("MoveY", dirY);
        }
        animator.SetFloat("Speed", direction.sqrMagnitude);
    }

    private bool checkIfPlayerInObj(List<BoxCollider2D> objColliders)
    {
        bool playerInObj = false;
        Bounds playerBounds = GetComponent<Collider2D>().bounds;
        foreach (BoxCollider2D cldr in objColliders)
        {
            Bounds objBounds = cldr.bounds;
            float colliderHalfWidth = cldr.size.x / 2;
            float colliderHalfHeight = cldr.size.y / 2;
            Vector3 minCldr = new Vector3(objBounds.center.x - colliderHalfWidth + cldr.offset.x, 
                objBounds.center.y - colliderHalfHeight + cldr.offset.y, 0);
            Vector3 maxCldr = new Vector3(objBounds.center.x + colliderHalfWidth + cldr.offset.x, 
                objBounds.center.y + colliderHalfHeight + cldr.offset.y, 0);
            objBounds.SetMinMax(minCldr, maxCldr);
            if (objBounds.Intersects(playerBounds))
                playerInObj = true;
        }
        return playerInObj;
    }

    private void ActivateAmulet()
    {
        bool playerInObj = false;
        switch (playerPrefs.currentTime)
        {
            case "Present":
                playerInObj = checkIfPlayerInObj(playerPrefs.pastObjectsCollider);
                break;
            case "Past":
                playerInObj = checkIfPlayerInObj(playerPrefs.presentObjectsCollider);
                break;
        }
        if (!playerInObj)
            StartCoroutine(coroutineValidAmulet());
        else
            StartCoroutine(coroutineInvalidAmulet());
    }

    private void Attack()
    {
        coroutine = StartCoroutine(coroutineAttack());
    }
    private void Rolling()
    {
        coroutine = StartCoroutine(coroutineRolling());
    }
    private void AddAmulet(GameObject amulet)
    {
        hasAmulet = true;
        amulet.GetComponent<ShowDialogue>().Interaction();
        amulet.SetActive(false);
    }

    public void Interaction(bool hasInventory)
    {
        if (targetItem != null)
        {
            targetItem.item.nameInScene = targetItem.transform.name;

            if (targetItem.name == "Amulet")
            {
                questHandler.CheckInteraction(targetItem.gameObject);
                AddAmulet(targetItem.gameObject);
                return;
            }

            GameData.PickedUpItem pickedUpItem = playerPrefs.locations[playerPrefs.location].PickedUpItems.Find(x => x.id==targetItem.item.id);
            int indexItem = playerPrefs.locations[playerPrefs.location].PickedUpItems.IndexOf(pickedUpItem);
            if (indexItem == -1)
            {
                playerPrefs.pickedUpItems++;
                int newId = playerPrefs.pickedUpItems;
                playerPrefs.locations[playerPrefs.location].PickedUpItems.Add(new GameData.PickedUpItem()
                {
                    id = newId,
                    locationOfPickingUp = playerPrefs.location, sceneOfPickingUp = playerPrefs.scene, name= targetItem.item.name, 
                    nameInScene = targetItem.item.nameInScene, lastPos=new Vector3(), selectedTime=playerPrefs.currentTime });
                targetItem.item.id = newId;
            }
            else
            {
                playerPrefs.locations[playerPrefs.location].PickedUpItems[indexItem].lastPos = new Vector3();
                playerPrefs.locations[playerPrefs.location].PickedUpItems[indexItem].locationOfDropping = -1;
                playerPrefs.locations[playerPrefs.location].PickedUpItems[indexItem].sceneOfDropping = -1;
            }
            questHandler.CheckInteraction(targetItem.gameObject);

            InventorySystem.instance.AddItem(targetItem.item);
            Destroy(targetItem.gameObject);
        }
        else if (targetNPC != null)
        {
            questHandler.CheckInteraction(targetNPC.gameObject);
            targetNPC.Interaction();
        }
        else if (targetChest != null)
        {
            questHandler.CheckInteraction(targetChest.gameObject);
            Chest.instance.Interaction();
        }
        /*
        if (targetKeyHole != null)
        {
            if (hasInventory)
            {
                targetKeyHole.Unlock(inventory.GetSelectItem());
            }
        }
        */
    }

    public void DropItem()
    {
        if (inventory.GetSelectItem() == null || inventory.GetSelectItem().itemType == ItemType.Quest)
            return;

        Item selectedItem = inventory.GetSelectItem();
        GameObject itemPrefab = Resources.Load<GameObject>("ItemsObjects/" + selectedItem.name);
        if (itemPrefab == null)
            return;
        if (!InventorySystem.instance.DropItem(inventory.GetSelectItem()))
            return;
        Vector3 spawnPosition = transform.position;
        Quaternion spawnRotation = Quaternion.identity;

        
        Transform parentForItem = null;
        switch (playerPrefs.currentTime)
        {
            case "Present":
                parentForItem = playerPrefs.folderWithItemsInPresent;
                break;
            case "Past":
                parentForItem = playerPrefs.folderWithItemsInPast;
                break;
        }
        GameObject newItemObject = Instantiate(itemPrefab, spawnPosition, spawnRotation, parentForItem);
        if(selectedItem.nameInScene!="")
            newItemObject.name = selectedItem.nameInScene;
        else
            newItemObject.name = selectedItem.name;


        GameData.PickedUpItem pickedUpItem = playerPrefs.locations[playerPrefs.location].PickedUpItems.Find(x => x.id == selectedItem.id);
        int indexItem = playerPrefs.locations[playerPrefs.location].PickedUpItems.IndexOf(pickedUpItem);
        if (indexItem == -1)
        {
            playerPrefs.pickedUpItems++;
            int newId = playerPrefs.pickedUpItems;
            playerPrefs.locations[playerPrefs.location].PickedUpItems.Add(new GameData.PickedUpItem()
            { id = newId, locationOfDropping = playerPrefs.location, sceneOfDropping = playerPrefs.scene, name = selectedItem.name, 
                nameInScene=selectedItem.nameInScene, lastPos = spawnPosition, selectedTime=playerPrefs.currentTime });
            newItemObject.GetComponent<ItemObject>().GetComponent<Item>().id = newId;
        }
        else
        {
            playerPrefs.locations[playerPrefs.location].PickedUpItems[indexItem].lastPos = spawnPosition;
            playerPrefs.locations[playerPrefs.location].PickedUpItems[indexItem].locationOfDropping = playerPrefs.location;
            playerPrefs.locations[playerPrefs.location].PickedUpItems[indexItem].sceneOfDropping = playerPrefs.scene;
        }
        
    }

    void MoveCharacter(float speed)
    {
        rigidbody2D.MovePosition(rigidbody2D.position + direction * speed * Time.deltaTime);
    }

    public virtual IEnumerator coroutineAttack()
    {
        isAttacking = true;
        animator.Play("Attack");
        yield return new WaitForSeconds(attackExitTime);
        isAttacking = false;
    }
    public virtual IEnumerator coroutineRolling()
    {
        isRolling = true;
        animator.SetBool("isRolling", true);
        yield return new WaitForSeconds(rollingExitTime);
        animator.SetBool("isRolling", false);
        isRolling = false;
    }

    public virtual IEnumerator coroutineValidAmulet()
    {
        activatedAmulet = true;
        animator.Play("AmuletFX");
        yield return new WaitForSeconds(1F);
        playerPrefs.activateAmulet();
        yield return new WaitForSeconds(0.5F);
        activatedAmulet = false;
    }
    public virtual IEnumerator coroutineInvalidAmulet()
    {
        activatedAmulet = true;
        animator.Play("AmuletFX_NotWorked");
        yield return new WaitForSeconds(1F);
        if (playerPrefs.gameObject.GetComponent<ShowDialogue>())
            playerPrefs.gameObject.GetComponent<ShowDialogue>().Interaction();
        //yield return new WaitForSeconds(0.5F);
        activatedAmulet = false;
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        switch(collider.tag)
        {
            case "NPC":
                if (collider.GetComponent<ShowDialogue>())
                    targetNPC = collider.GetComponent<ShowDialogue>();
                break;
            case "Item":
                if (collider.GetComponent<ItemObject>())
                    targetItem = collider.GetComponent<ItemObject>();
                break;
            case "Chest":
                if (collider.GetComponent<Chest>())
                    targetChest = collider.GetComponent<Chest>();
                break;
        }
    }
    void OnTriggerExit2D(Collider2D collider)
    {
        switch (collider.tag)
        {
            case "NPC":
                targetNPC = null;
                break;
            case "Item":
                targetItem = null;
                break;
            case "Chest":
                targetChest = null;
                break;
        }
    }

    public void SetHealth(int hp)
    {
        this.health = hp;
        healthBar.SetHealth(this.health);
    }

    public void Hit(float damage)
    {
        if (isRolling || lockPlayer)
            return;
        health--;
        healthBar.SetHealth(health);
        animator.Play("Hit");
        Debug.Log("Player health = " + health); 
        if (health <= 0)
        {
            StartCoroutine(Death());
            animator.SetBool("IsAttacking", false);
        }
    }

    IEnumerator Death()
    {
        animator.Play("Death"); 
        AnimatorStateInfo animatorStateInfo = animator.GetCurrentAnimatorStateInfo(0);
        yield return new WaitForSeconds(animatorStateInfo.length + .5F);
        transform.gameObject.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;

        deathMenu.SetActive(true);
    }

    public void LoadData(GameData gameData)
    {
        SetHealth(gameData.general.HP);
        GameData.Loc1 loc1 = (GameData.Loc1)gameData.locations[0];
        hasAmulet = loc1.PickedAmulet;

        for (int i = 0; i < inventory.inventorySlots.Length; i++)
        {
            if (gameData.inventory.items[i] == null)
                continue;
            if (gameData.inventory.items[i].name != "" || gameData.inventory.items[i].count != 0)
            {
                Item savedItem = Resources.Load<Item>("Items/" + gameData.inventory.items[i].name);
                savedItem.id = gameData.inventory.items[i].id;
                savedItem.nameInScene = gameData.inventory.items[i].nameInScene;
                for (int j = 1; j <= gameData.inventory.items[i].count; j++)
                {
                    inventory.AddItem(savedItem);
                }
            }
        }
    }

    public void SaveData(ref GameData gameData)
    {
        gameData.general.HP = this.health;

        for (int i = 0; i < inventory.inventorySlots.Length; i++)
        {
            if (gameData.inventory.items[i] == null)
                gameData.inventory.items[i] = new GameData.Item();
            if (inventory.inventorySlots[i].GetComponentInChildren<InventoryItemSlot>() != null)
            {
                gameData.inventory.items[i].id = inventory.inventorySlots[i].GetComponentInChildren<InventoryItemSlot>().item.id;
                gameData.inventory.items[i].name = inventory.inventorySlots[i].GetComponentInChildren<InventoryItemSlot>().item.name;
                gameData.inventory.items[i].nameInScene = inventory.inventorySlots[i].GetComponentInChildren<InventoryItemSlot>().item.nameInScene;
                gameData.inventory.items[i].count = inventory.inventorySlots[i].GetComponentInChildren<InventoryItemSlot>().count;
            }
            else
                gameData.inventory.items[i] = new GameData.Item();
        }
    }
}
