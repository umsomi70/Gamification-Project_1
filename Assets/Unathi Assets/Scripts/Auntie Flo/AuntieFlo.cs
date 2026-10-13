using System.Collections;
using UnityEngine;
using TMPro;

public class AuntieFlo : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private GameObject assistantPanel;
    [SerializeField] private TMP_Text dialogueText;

    [Header("Animation")]
    [SerializeField] private Animator animator;

    [Header("Animation Parameters")]
    [SerializeField] private string activeBool = "isActive";

    [Header("Settings")]
    [SerializeField] private float displayDuration = 10f;

    private Coroutine currentDialogueCoroutine;
    private bool isShowing;

    private void Start()
    {
        HideInstant();
    }

    /// <summary>
    /// Shows assistant with dialogue.
    /// </summary>
    private void ShowAssistant(string message)
    {
        if (assistantPanel != null)
            assistantPanel.SetActive(true);

        if (dialogueText != null)
            dialogueText.text = message; // Initial message sets the text directly

        if (animator != null)
            animator.SetBool(activeBool, true);

        isShowing = true;
    }

    /// <summary>
    /// Hides assistant.
    /// </summary>
    private void HideAssistant()
    {
        if (animator != null)
            animator.SetBool(activeBool, false);

        if (assistantPanel != null)
            assistantPanel.SetActive(false);

        isShowing = false;
    }

    /// <summary>
    /// Hides instantly on start.
    /// </summary>
    private void HideInstant()
    {
        if (assistantPanel != null)
            assistantPanel.SetActive(false);

        if (animator != null)
            animator.SetBool(activeBool, false);

        isShowing = false;
    }

    /// <summary>
    /// Main public function.
    /// UPDATED: If assistant is already active, new messages are 
    /// appended underneath on a new line, and the auto-hide timer resets.
    /// </summary>
    public void CallAssistant(string message)
    {
        if (isShowing)
        {
            // FIX: Append the text on a brand new line instead of erasing what was there
            if (dialogueText != null)
            {
                dialogueText.text += "\n" + message;
            }

            // Reset the auto-hide timer so the player has fresh time to read the combined messages
            if (currentDialogueCoroutine != null)
            {
                StopCoroutine(currentDialogueCoroutine);
            }

            currentDialogueCoroutine = StartCoroutine(HideRoutine());
            return;
        }

        // Otherwise show assistant normally with the first message
        ShowAssistant(message);
        currentDialogueCoroutine = StartCoroutine(HideRoutine());
    }

    /// <summary>
    /// Handles auto hiding.
    /// </summary>
    private IEnumerator HideRoutine()
    {
        yield return new WaitForSeconds(displayDuration);

        HideAssistant();

        currentDialogueCoroutine = null;
    }
}