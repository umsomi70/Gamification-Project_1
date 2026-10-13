using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum DayPhase
{
    Morning,
    InProgress,
    Summary
}

public class DayManager : MonoBehaviour
{
    public DayPhase currentPhase = DayPhase.Morning;

    [Header("School Supplies")]
    [SerializeField] ItemData periodItem;

    public int currentDay = 1;

    [Header("References")]
    [SerializeField] UIManager ui;
    [SerializeField] CycleManager cycleManager;
    [SerializeField] ChildManager child;
    [SerializeField] StressManager stressManager;
    [SerializeField] InventoryManager inventory;
    [SerializeField] CanvasManager canvas;

    [Header("End Summary UI")]
    [SerializeField] FadeManager fadeManager;
    [SerializeField] GameObject summaryPanel;
    [SerializeField] Text endOfDayText;

    private bool tasksConsumed;

    void Start()
    {
        UpdateUI();
        StartNewDayContent();
    }

    public void NextDay()
    {
        currentDay++;
        tasksConsumed = false;

        if (canvas.inventory)
        {
            canvas.ToggleInventory();
        }

        cycleManager.OnNewDay(currentDay);
        GameManager.instance.OnNewDay();

        UpdateUI();
        StartNewDayContent();
    }

    void StartNewDayContent()
    {
        bool isWeekend = IsWeekend();

        // 1. SCHOOL EVENTS (ONLY WEEKDAYS)
        if (!isWeekend)
        {
            EventManager.instance.GenerateSchoolEvents(GameManager.instance.difficultyLevel);
            GameManager.instance.SetSchoolEvents(EventManager.instance.schoolEventsToday);
        }
        else
        {
            GameManager.instance.SetSchoolEvents(new List<SchoolEvent>());
        }

        // 2. PERIOD EVENTS (EVERY DAY IF ACTIVE)
        if (cycleManager.isOnPeriod)
        {
            EventManager.instance.GeneratePeriodEvents(GameManager.instance.difficultyLevel);
            GameManager.instance.SetPeriodEvents(EventManager.instance.periodEventsToday);
        }
        else
        {
            GameManager.instance.SetPeriodEvents(new List<SchoolEvent>());
        }

        // 3. TASK SYSTEM (Now correctly passes both pools down to task evaluation)
        TaskManager.instance.NewDay(
            GameManager.instance.schoolEventsToday,
            GameManager.instance.periodEventsToday,
            cycleManager.isOnPeriod
        );
    }

    public bool IsWeekend()
    {
        int dayIndex = (currentDay - 1) % 7;
        return dayIndex == 5 || dayIndex == 6;
    }

    public void SendToSchool()
    {
        if (IsWeekend())
            return;

        StartCoroutine(SchoolRoutine());
    }

    IEnumerator SchoolRoutine()
    {
        child.ToggleLocation();
        currentPhase = DayPhase.InProgress;

        yield return StartCoroutine(fadeManager.FadeOut());
        yield return StartCoroutine(fadeManager.FadeIn());

        ResolveSchoolDay();

        summaryPanel.SetActive(true);
        currentPhase = DayPhase.Summary;
    }

    public void EndDay()
    {
        if (!IsWeekend())
            return;

        StartCoroutine(EndDayRoutine());
    }

    IEnumerator EndDayRoutine()
    {
        currentPhase = DayPhase.InProgress;

        yield return StartCoroutine(fadeManager.FadeOut());
        yield return StartCoroutine(fadeManager.FadeIn());

        ResolveSchoolDay();

        summaryPanel.SetActive(true);
        currentPhase = DayPhase.Summary;
    }

    // =====================================================
    // FIXED DAY RESOLUTION ENGINE
    // =====================================================
    void ResolveSchoolDay()
    {
        bool isWeekend = IsWeekend();

        int totalEventTasks = 0;
        int completedEventTasks = 0;

        // Dynamically tallying how many resource events were actualized today
        foreach (var t in TaskManager.instance.tasks)
        {
            if (!t.isManualTask) // Filters out manual status tasks like "Talk to Daughter"
            {
                totalEventTasks++;
                if (t.isCompleted)
                {
                    completedEventTasks++;
                }
            }
        }

        // Calculate score solely on active event requirements met
        float score = totalEventTasks == 0 ? 1f : (float)completedEventTasks / totalEventTasks;

        string breakdown = TaskManager.instance.GetAllTasksBreakdown();
        string resultText = "";

        if (score >= 0.75f)
        {
            stressManager.AddStress(-1);

            resultText = isWeekend
                ? "It was a smooth weekend day."
                : "She had a great day at school.";

            GameManager.instance.SuccessfulSchoolDay1();
        }
        else if (score >= 0.4f)
        {
            stressManager.AddStress(1);

            resultText = isWeekend
                ? "It was an demanding day."
                : "It was an okay day at school.";

            GameManager.instance.SuccessfulSchoolDay2();
        }
        else
        {
            stressManager.SetStress(4);

            resultText = isWeekend
                ? "It was an incredibly rough day."
                : "She struggled a lot today at school...";

            GameManager.instance.FailedSchoolDay();
        }

        endOfDayText.text = resultText + "\n\n" + breakdown;
    }

    public void OnNextDayFromSummary()
    {
        if (!tasksConsumed)
        {
            TaskManager.instance.ConsumeCompletedTaskItems(inventory);
            tasksConsumed = true;
        }

        summaryPanel.SetActive(false);

        if (!IsWeekend())
        {
            child.ToggleLocation();
        }

        NextDay();
    }

    void UpdateUI()
    {
        ui.UpdateDayText(GetFormattedDate(currentDay));
    }

    string GetFormattedDate(int day)
    {
        string[] daysOfWeek = { "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday", "Sunday" };
        int dayIndex = (day - 1) % 7;
        int dayOfMonth = ((day - 1) % 31) + 1;

        return daysOfWeek[dayIndex] + " the " + dayOfMonth + GetDaySuffix(dayOfMonth);
    }

    string GetDaySuffix(int day)
    {
        if (day >= 11 && day <= 13) return "th";
        switch (day % 10)
        {
            case 1: return "st";
            case 2: return "nd";
            case 3: return "rd";
            default: return "th";
        }
    }
}