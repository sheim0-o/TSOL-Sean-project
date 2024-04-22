using UnityEngine;

public class DialogueException : System.Exception
{
    // Метод обработки ошибок
    public DialogueException(string message)
        : base(message)
    {
    }
}

public class DialogueSequencer
{
    public delegate void DialogueCallback(Dialogue dialogue);
    public delegate void DialogueTypeCallback(DialogueType type);

    public DialogueCallback OnDialogueStart;
    public DialogueCallback OnDialogueEnd;
    public DialogueTypeCallback OnDialogueNodeStart;
    public DialogueTypeCallback OnDialogueNodeEnd;

    private Dialogue m_CurrentDialogue;
    private DialogueType m_CurrentType;

    private GameObject player;

    // Обработка начала диалога
    public void StartDialogue(Dialogue dialogue)
    {
        if (m_CurrentDialogue == null)
        {
            if(player==null)
                player = GameObject.FindWithTag("Player");
            player.GetComponent<Player>().lockPlayer = true;
            m_CurrentDialogue = dialogue;
            OnDialogueStart?.Invoke(m_CurrentDialogue);
            StartDialogueType(dialogue.FirstType);

        }
        else
        {
            throw new DialogueException("Can't start a dialogue when another is already running.");
        }
    }

    // Обработка окончания диалога
    public void EndDialogue(Dialogue dialogue)
    {
        if (m_CurrentDialogue == dialogue)
        {
            if (player == null)
                player = GameObject.FindWithTag("Player");
            player.GetComponent<Player>().lockPlayer = false;
            StopDialogueType(m_CurrentType);
            OnDialogueEnd?.Invoke(m_CurrentDialogue);
            m_CurrentDialogue = null;
        }
        else
        {
            throw new DialogueException("Trying to stop a dialogue that ins't running.");
        }
    }

    private bool CanStartType(DialogueType type)
    {
        return (m_CurrentType == null || type == null || m_CurrentType.CanBeFollowedByType(type));
    }

    public void StartDialogueType(DialogueType type)
    {
        if (CanStartType(type))
        {
            StopDialogueType(m_CurrentType);

            m_CurrentType = type;

            if (m_CurrentType != null)
            {
                OnDialogueNodeStart?.Invoke(m_CurrentType);
            }
            else
            {
                EndDialogue(m_CurrentDialogue);
            }
        }
        else
        {
            throw new DialogueException("Failed to start dialogue node.");
        }
    }

    private void StopDialogueType(DialogueType type)
    {
        if (m_CurrentType == type)
        {
            OnDialogueNodeEnd?.Invoke(m_CurrentType);
            m_CurrentType = null;
        }
        else
        {
            throw new DialogueException("Trying to stop a dialogue node that ins't running.");
        }
    }
}