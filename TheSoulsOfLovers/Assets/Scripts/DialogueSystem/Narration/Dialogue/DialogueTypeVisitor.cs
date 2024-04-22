public interface DialogueTypeVisitor
{
    void Visit(OneLineDialogueType type);
    void Visit(ChoiceDialogueType type);
}
