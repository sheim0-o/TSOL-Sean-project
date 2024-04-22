using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Narration/Dialogue/Dialogue")] 
public class Dialogue : ScriptableObject
{
    [SerializeField]
    private DialogueType m_FirstType;
    public DialogueType FirstType => m_FirstType;
}