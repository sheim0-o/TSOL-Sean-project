using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class ShowDialogue : MonoBehaviour
{
    [SerializeField] UnityEvent m_OnInteraction;
    public enum DialogueType
    {
        None,
        NPC
    };

    public DialogueType dialogueType = DialogueType.None;
    private TextMeshProUGUI textMesh;
    private GameObject player;
    private float NpcSpeed;

    private string DTNPC = "Для начала диалога нажмите E";

    private void Start()
    {
        player = GameObject.FindWithTag("Player");
        textMesh = GameObject.FindWithTag("InteractableText").GetComponent<TextMeshProUGUI>();
        if ((GetComponent<FollowPath>() as FollowPath) != null)
            NpcSpeed = GetComponent<FollowPath>().speed;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject == player)
        {

            if ((GetComponent<FollowPath>() as FollowPath) != null)
                GetComponent<FollowPath>().speed = 0;
            
            switch(dialogueType)
            {
                case DialogueType.NPC:
                    textMesh.text = "Для начала диалога нажмите E";
                    break;
            }
        }
    }


    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject == player) {
            if ((GetComponent<FollowPath>() as FollowPath) != null)
                GetComponent<FollowPath>().speed = NpcSpeed;

            textMesh.text = "";
        }
    }

    public void Interaction()
    {
        textMesh.text = "";
        m_OnInteraction.Invoke();
    }
    public void SetNewDialog(Dialogue dialogue)
    {
        var property = m_OnInteraction.GetPersistentTarget(0).GetType().GetField("startOfDialogue");
        if (property != null)
            property.SetValue(m_OnInteraction.GetPersistentTarget(0), dialogue);
    }
}