using System;
using System.Linq;
using UnityEngine;

[Serializable]
public class DialogueChoice
{
	[SerializeField]
	private string m_ChoicePreview;
	[SerializeField]
	private DialogueType m_ChoiceType;

	public string ChoicePreview => m_ChoicePreview;
	public DialogueType ChoiceType => m_ChoiceType;
}


[CreateAssetMenu(menuName = "Scriptable Objects/Narration/Dialogue/Types/Choice")]
public class ChoiceDialogueType : DialogueType
{
	[SerializeField]
	private DialogueChoice[] m_Choices;
	public DialogueChoice[] Choices => m_Choices;


	public override bool CanBeFollowedByType(DialogueType type)
	{
		return m_Choices.Any(x => x.ChoiceType == type);
	}

	public override void Accept(DialogueTypeVisitor visitor)
	{
		visitor.Visit(this);
	}
}
