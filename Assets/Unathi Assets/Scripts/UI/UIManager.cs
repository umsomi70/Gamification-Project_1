using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [Header("Rewards UI")]
    public Text xPPointsText;
    public Text caregiverPointsText;

    [SerializeField] GameManager gameManager;

    [Header("Inventory UI")]
    public Image[] slotImages;
    public Text[] slotTexts;
    public Image[] slotBorders;

    [SerializeField] InventoryManager inventory;

    [Header("Day System UI")]
    public Text dayText;
    public Text infoText;

    Coroutine messageCoroutine;

    [SerializeField] DayManager dayManager;

    [Header("Cycle UI")]
    public Text cycleProgressText;

    [SerializeField] CycleManager cycleManager;

    [Header("Stress UI")]
    public Image stressDisplay;

    [SerializeField] Image stressFillImage;

    public Sprite[] stressSprites;

    public Slider stressSlider;

    [SerializeField] StressManager stressManager;

    [Header("Interact UI")]
    public GameObject interactUI;

    [SerializeField] bool interactUIActive = false;

    [SerializeField] GameObject sendToSchoolButton;
    [SerializeField] GameObject nextDayButton;

    [Header("Relationship UI")]
    [SerializeField] Relationship relationship;
    public Slider relationSlider;

    public Text coinsText;

    [SerializeField] Text taskListText;

    void Start()
    {
        UpdateStressUI();

        stressSlider.maxValue = 4;
        relationSlider.maxValue = relationship.maxRelation;

        interactUIActive = false;
    }

    void Update()
    {
        UpdateStressUI();
        UpdateInventoryUI();
        UpdateRewardsUI();
        UpdateCycleProgressText();
        UpdateTaskUI();
        UpdateRelationshipUI();

        interactUI.SetActive(interactUIActive);

        sendToSchoolButton.SetActive(!dayManager.IsWeekend());
        nextDayButton.SetActive(dayManager.IsWeekend());

        coinsText.text = "Coins: " + gameManager.coins.ToString();
    }

    // =========================================================
    // STRESS UI
    // =========================================================

    void UpdateStressUI()
    {
        if (stressManager == null ||
            stressSprites.Length == 0 ||
            stressDisplay == null)
            return;

        int stressLevel = stressManager.stressLevel;

        stressLevel =
            Mathf.Clamp(stressLevel, 0, stressSprites.Length - 1);

        // Update sprite
        stressDisplay.sprite = stressSprites[stressLevel];

        // Update slider
        stressSlider.value = stressLevel;

        // Slider color
        float t = stressLevel / (float)stressSlider.maxValue;

        Color color = Color.Lerp(Color.green, Color.red, t);

        stressFillImage.color = color;
    }

    // =========================================================
    // INVENTORY UI
    // =========================================================

    void UpdateInventoryUI()
    {
        for (int i = 0; i < inventory.slots.Length; i++)
        {
            var slot = inventory.slots[i];

            if (slot.item != null)
            {
                // ICON
                slotImages[i].sprite = slot.item.icon;
                slotImages[i].enabled = true;

                // QUANTITY
                slotTexts[i].text = slot.quantity.ToString();
            }
            else
            {
                slotImages[i].enabled = false;
                slotTexts[i].text = "";
            }

            // SELECTED SLOT
            slotBorders[i].enabled =
                (i == inventory.selectedSlot);
        }
    }

    // =========================================================
    // REWARDS UI
    // =========================================================

    void UpdateRewardsUI()
    {
        if (gameManager == null)
            return;

        xPPointsText.text =  "XP: " + gameManager.xPPoints.ToString();

        caregiverPointsText.text = "Caregiver: " + gameManager.caregiverPoints.ToString();
    }

    void UpdateRelationshipUI()
    {
        if (relationship == null) return;

        relationSlider.value = relationship.relationship;
    }

    // =========================================================
    // CYCLE UI
    // =========================================================

    public void UpdateCycleProgressText()
    {
        if (cycleManager == null)
            return;

        // Currently on period
        if (cycleManager.isOnPeriod)
        {
            cycleProgressText.text =
                "Cycle Day: " +
                cycleManager.periodDay +
                "/" +
                cycleManager.periodLength;
        }
        else
        {
            int daysRemaining =
                cycleManager.predictedNextPeriod -
                dayManager.currentDay;

            cycleProgressText.text =
                "Next Cycle In: " +
                daysRemaining +
                " Days";
        }
    }

    // =========================================================
    // MESSAGE SYSTEM
    // =========================================================

    public void ShowMessage(string message)
    {
        // Stop previous message coroutine
        if (messageCoroutine != null)
        {
            StopCoroutine(messageCoroutine);
        }

        // Start new one
        messageCoroutine =
            StartCoroutine(DisplayMessage(message));
    }

    public IEnumerator DisplayMessage(string message)
    {
        infoText.text = message;

        yield return new WaitForSeconds(5f);

        infoText.text = "";

        messageCoroutine = null;
    }

    // =========================================================
    // INTERACT UI
    // =========================================================

    public void InteractUI()
    {
        interactUIActive = !interactUIActive;
    }

    // =========================================================
    // DAY UI
    // =========================================================

    public void UpdateDayText(string formattedDay)
    {
        dayText.text = formattedDay;
    }

    public void UpdateTaskUI()
    {
        if (TaskManager.instance == null)
            return;

        string display = "";

        foreach (var task in TaskManager.instance.tasks)
        {
            string status = task.isCompleted ? "(DONE)" : "...";

            display += status + " " + task.taskName + "\n";
        }

        taskListText.text = display;
    }
}