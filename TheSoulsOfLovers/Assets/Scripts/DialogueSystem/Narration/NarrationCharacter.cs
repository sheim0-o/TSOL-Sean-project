// Информация о том, какой нпс принимает участие в диалоге

using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Narration/Character")]
public class NarrationCharacter : ScriptableObject
{
	[SerializeField]
	private string m_CharacterName;

	public string CharacterName => m_CharacterName;
}
