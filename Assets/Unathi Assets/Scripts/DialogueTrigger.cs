using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    [Header("Dialogue")]
    public DialogueData[] stressDialogues;

    DialogueManager dialogueManager;

    [Header("References")]
    public StressManager stressManager;

    void Start()
    {
        dialogueManager = FindAnyObjectByType<DialogueManager>();
    }

    public void TriggerDialogue()
    {
        if (dialogueManager == null)
        {
            Debug.LogWarning("DialogueManager missing!");
            return;
        }

        int stress = stressManager.stressLevel;

        // Clamp to array size
        stress = Mathf.Clamp(stress, 0, stressDialogues.Length - 1);

        DialogueData selectedDialogue = stressDialogues[stress];

        if (selectedDialogue != null)
        {
            dialogueManager.StartDialogue(selectedDialogue);
        }
        else
        {
            Debug.LogWarning("No dialogue assigned for this stress level!");
        }
    }
}