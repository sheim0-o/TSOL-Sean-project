using UnityEngine;

public abstract class DialogueType : ScriptableObject
{
	[SerializeField]
	private NarrationLine m_DialogueLine;

	public NarrationLine DialogueLine => m_DialogueLine;

	public abstract bool CanBeFollowedByType(DialogueType type);
	public abstract void Accept(DialogueTypeVisitor visitor);
}
