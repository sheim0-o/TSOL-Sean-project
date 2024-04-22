using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Narration/Dialogue/Types/OneLine")]
public class OneLineDialogueType : DialogueType
{
	[SerializeField]
	private DialogueType m_NextType;
	public DialogueType NextType => m_NextType;


	public override bool CanBeFollowedByType(DialogueType type)
	{
		return m_NextType == type;
	}

	public override void Accept(DialogueTypeVisitor visitor)
	{
		visitor.Visit(this);
	}
}
