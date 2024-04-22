using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Narration/Dialogue/Dialogue Channel")]
public class DialogueChannel : ScriptableObject
{
    public Dialogue startOfDialogue = null;
    public delegate void DialogueCallback(Dialogue dialogue);
    public DialogueCallback OnDialogueRequested;
    public DialogueCallback OnDialogueStart;
    public DialogueCallback OnDialogueEnd;

    public delegate void DialogueTypeCallback(DialogueType type);
    public DialogueTypeCallback OnDialogueTypeRequested;
    public DialogueTypeCallback OnDialogueTypeStart;
    public DialogueTypeCallback OnDialogueTypeEnd;

    public void RaiseRequestDialogue(Dialogue dialogue)
    {
        if (startOfDialogue != null)
            dialogue = startOfDialogue;
        OnDialogueRequested?.Invoke(dialogue);
        if (startOfDialogue != null)
            startOfDialogue = null;
    }

    public void RaiseDialogueStart(Dialogue dialogue)
    {
        OnDialogueStart?.Invoke(dialogue);
    }

    public void RaiseDialogueEnd(Dialogue dialogue)
    {
        OnDialogueEnd?.Invoke(dialogue);
    }

    public void RaiseRequestDialogueType(DialogueType type)
    {
        OnDialogueTypeRequested?.Invoke(type);
    }

    public void RaiseDialogueTypeStart(DialogueType type)
    {
        OnDialogueTypeStart?.Invoke(type);
    }

    public void RaiseDialogueTypeEnd(DialogueType type)
    {
        OnDialogueTypeEnd?.Invoke(type);
    }
}
