using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class QuestHandler : MonoBehaviour
{
    private PlayerPrefs playerPrefs;
    private GameObject player;
    [SerializeField] public List<Quest> quests;

    [System.Serializable] public class Quest
    {
        [Header("General")][Space]
        public GameObject questObject;
        public QuestType type = QuestType.None;
        public string nameOfBool;
        public FollowUpAction followUpAction = FollowUpAction.None;
        [Header("Other info")][Space]
        public GameObject followUpActionFor;
        public Dialogue changeDialogueUsing;
        public bool instantlySetNewDialogue;
        public Item questItem;
        public List<string> listOfBoolsToActivate;
    }
    public enum QuestType
    {
        None,
        PickedItem,
        TalkingWithNPC,
        OpenedChest
    };
    public enum FollowUpAction
    {
        None,
        RemoveObject,
        ChangeDialogue,
        DeleteItem
    };


    public void CheckInteraction(GameObject gameObject)
    {
        if (gameObject == null)
            return;
        List<Quest> selectedQuests = quests.Where(x => x.questObject == gameObject).ToList();
        if (selectedQuests.Count == 0 || playerPrefs == null)
            return;

        foreach(Quest curQuest in selectedQuests)
        {
            var property = playerPrefs.locations[playerPrefs.location].GetType().GetField(curQuest.nameOfBool);
            if (property != null || curQuest.nameOfBool == "")
            {
                bool activatePerform = true;
                foreach (string nameOfBoolToActivate in curQuest.listOfBoolsToActivate)
                {
                    var boolToActivate = playerPrefs.locations[playerPrefs.location].GetType().GetField(nameOfBoolToActivate);
                    if (boolToActivate == null || (bool)boolToActivate.GetValue(playerPrefs.locations[playerPrefs.location]) != true)
                        activatePerform = false;
                }
                if(activatePerform)
                {
                    bool ifPerformIsSuccessful = PerformAction(curQuest);
                    if (property != null && ifPerformIsSuccessful && (bool)property.GetValue(playerPrefs.locations[playerPrefs.location]) != true)
                    {
                        property.SetValue(playerPrefs.locations[playerPrefs.location], true);
                    }
                }
            }
        }
    }
    public void Start()
    {
        GameObject playerPrefsObject = GameObject.FindWithTag("PlayerPrefs");
        if (playerPrefsObject!=null)
            playerPrefs = playerPrefsObject.GetComponent<PlayerPrefs>();
        player = GameObject.FindWithTag("Player");

        foreach (Quest quest in quests)
        {
            var property = playerPrefs.locations[playerPrefs.location].GetType().GetField(quest.nameOfBool);
            if (quest.nameOfBool=="" || (property != null && (bool)property.GetValue(playerPrefs.locations[playerPrefs.location]) == true))
            {
                if (quest.type == QuestType.TalkingWithNPC)
                    quest.instantlySetNewDialogue = true;
                bool activatePerform = true;
                foreach (string nameOfBoolToActivate in quest.listOfBoolsToActivate)
                {
                    var boolToActivate = playerPrefs.locations[playerPrefs.location].GetType().GetField(nameOfBoolToActivate);
                    if (boolToActivate != null && (bool)boolToActivate.GetValue(playerPrefs.locations[playerPrefs.location]) == false)
                        activatePerform = false;
                }
                if (quest.followUpAction == FollowUpAction.ChangeDialogue)
                    activatePerform = false;

                if (activatePerform)
                {
                    bool ifPerformIsSuccessful = PerformAction(quest);
                    if (!ifPerformIsSuccessful)
                    {
                        Debug.Log("Ошибка в прогрузке сохраненных миссий!");
                    }
                }
            }
        }
    }

    private bool PerformAction(Quest quest)
    {
        switch(quest.followUpAction)
        {
            case (FollowUpAction.RemoveObject):
                RemoveBarrier(quest.followUpActionFor);
                return true;
            case (FollowUpAction.ChangeDialogue):
                if (!quest.instantlySetNewDialogue)
                    quest.instantlySetNewDialogue = true;
                else
                    ChangeDialogue(quest.questObject, quest.changeDialogueUsing);
                return true;
            case (FollowUpAction.DeleteItem):
                DeleteItem(quest.questItem);
                return true;
            case (FollowUpAction.None):
                return true;
        }
        return false;
    }
    private void ChangeDialogue(GameObject npc, Dialogue newDialogue)
    {
        npc.GetComponent<ShowDialogue>().SetNewDialog(newDialogue);
    }
    private void RemoveBarrier(GameObject barrier)
    {
        barrier.SetActive(false);
    }
    private void DeleteItem(Item questItem)
    {
        InventorySlot[] ISSlots = player.GetComponent<Player>().inventory.inventorySlots;
        foreach(InventorySlot ISSlot in ISSlots)
        {
            if (ISSlot == null || ISSlot.GetComponentInChildren<InventoryItemSlot>() == null || ISSlot.GetComponentInChildren<InventoryItemSlot>().item == null)
                continue;
            if (ISSlot.GetComponentInChildren<InventoryItemSlot>().item.name == questItem.name)
            {
                player.GetComponent<Player>().inventory.DeleteItem(ISSlot);
            }
        }
    }
}
