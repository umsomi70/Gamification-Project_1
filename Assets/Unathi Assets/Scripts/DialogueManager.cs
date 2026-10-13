using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    [Header("UI")]
    public GameObject dialoguePanel;

    public Text parentText;
    public Text childText;

    public Button[] choiceButtons;

    [Header("References")]
    public StressManager stressManager;
    public Relationship relationship;

    DialogueData currentDialogue;

    void Start()
    {
        dialoguePanel.SetActive(false);
    }

    public void StartDialogue(DialogueData dialogue)
    {
        dialoguePanel.SetActive(true);
        currentDialogue = dialogue;

        ShowDialogue();
    }

    void ShowDialogue()
    {
        // Show BOTH lines at once
        parentText.text = "You: " + currentDialogue.parentLine;
        childText.text = "Daughter: " + currentDialogue.childLine;

        // Setup choices
        for (int i = 0; i < choiceButtons.Length; i++)
        {
            if (i < currentDialogue.choices.Length)
            {
                choiceButtons[i].gameObject.SetActive(true);

                int index = i;

                choiceButtons[i].GetComponentInChildren<Text>().text =
                    currentDialogue.choices[i].choiceText;

                choiceButtons[i].onClick.RemoveAllListeners();
                choiceButtons[i].onClick.AddListener(() => ChooseOption(index));
            }
            else
            {
                choiceButtons[i].gameObject.SetActive(false);
            }
        }
    }

    void ChooseOption(int index)
    {
        var choice = currentDialogue.choices[index];

        if (TaskManager.instance.TryCheckTaskStatus("Talk To Daughter", out bool isComplete))
        {
            if (!isComplete)
            {
                if (choice.stressEffect < 0)
                {
                    TaskManager.instance.CompleteManualTask("Talk To Daughter");
                    GameManager.instance.AddCaregiverPoints(3);
                    GameManager.instance.AddCoins(5);
                    GameManager.instance.AddRelation(1);
                    GameManager.instance.AddXPPoints(5);
                }
            }
            else
            {
                Debug.Log("Task is complete!");
            }
        }
        else
        {
            Debug.Log("Task does not exist");
        }


            // Apply stress effect
            stressManager.AddStress(choice.stressEffect);
        relationship.AddRelation(choice.relationshipEffect);

        // Go to next dialogue OR close
        if (choice.nextDialogue != null)
        {
            currentDialogue = choice.nextDialogue;
            ShowDialogue();
        }
        else
        {
            EndDialogue();
        }
    }

    void EndDialogue()
    {
        dialoguePanel.SetActive(false);
    }
}