using UnityEngine;

[CreateAssetMenu(fileName = "New Dialogue", menuName = "Dialogue/Dialogue")]
public class DialogueData : ScriptableObject
{
    [Header("Dialogue Lines")]
    [TextArea] public string parentLine;
    [TextArea] public string childLine;

    [Header("Choices")]
    public DialogueChoice[] choices;
}

[System.Serializable]
public class DialogueChoice
{
    public string choiceText;
    public int stressEffect;
    public int relationshipEffect;

    public DialogueData nextDialogue; // optional (for chaining)
}