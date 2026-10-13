using System.Collections.Generic;
using UnityEngine;

public class TaskManager : MonoBehaviour
{
    public static TaskManager instance;

    [SerializeField] InventoryManager inventory;
    [SerializeField] DayManager dayManager;                        

    public List<DailyTask> tasks = new List<DailyTask>();

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
    }

    void Update()
    {
        EvaluateTasks();
    }


    // NEW DAY

    // UPDATED: Now accepts both event pools explicitly
    public void NewDay(List<SchoolEvent> schoolEvents, List<SchoolEvent> periodEvents, bool isOnPeriod)
    {
        bool isWeekend = dayManager.IsWeekend();

        tasks.Clear();

        AddManualTask("Talk To Daughter");
        AddManualTask("Check Supplies");

        if(!isWeekend)
        AddManualTask("Send To School");

        // Generate tasks for whatever events exist today
        GenerateEventTasks(schoolEvents);

        if (isOnPeriod)
        {
            GenerateEventTasks(periodEvents);
        }
    }


    // MANUAL TASKS (BASE GAMEPLAY ACTIONS)

    public void AddManualTask(string taskName)
    {
        tasks.Add(new DailyTask
        {
            taskName = taskName,
            isManualTask = true,
            manualCompleted = false
        });
    }

    public void CompleteManualTask(string taskName)
    {
        foreach (var task in tasks)
        {
            if (task.taskName == taskName && task.isManualTask)
            {
                task.manualCompleted = true;
                task.isCompleted = true;
                break;
            }
        }
    }


    // EVENT TASKS (RESOURCE BASED)

    // EVENT TASKS (RESOURCE BASED) - UNCHANGED BUT SHOWN FOR CONTEXT
    private void GenerateEventTasks(List<SchoolEvent> events)
    {
        if (events == null) return;

        foreach (SchoolEvent e in events)
        {
            if (e == null || e.requirements == null)
                continue;

            string requirementText = "";

            for (int i = 0; i < e.requirements.Length; i++)
            {
                var req = e.requirements[i];

                if (req == null || req.item == null)
                    continue;

                requirementText += $"{req.amount}x {req.item.itemName}";

                if (i < e.requirements.Length - 1)
                    requirementText += ", ";
            }

            string fullTaskName = e.eventName;

            if (!string.IsNullOrEmpty(requirementText))
            {
                fullTaskName += $" ({requirementText})";
            }

            tasks.Add(new DailyTask
            {
                taskName = fullTaskName,
                requirements = e.requirements,
                isManualTask = false // This tells the game it requires items to complete!
            });
        }
    }


    // CORE EVALUATION

    public void EvaluateTasks()
    {
        Dictionary<ItemData, int> pool = inventory.GetSnapshot();

        foreach (var task in tasks)
        {
            // MANUAL TASKS → DO NOT OVERRIDE
            if (task.isManualTask)
                continue;

            if (task.requirements == null || task.requirements.Length == 0)
                continue;

            bool allMet = true;

            foreach (var req in task.requirements)
            {
                if (req == null || req.item == null)
                    continue;

                pool.TryGetValue(req.item, out int available);

                if (available < req.amount)
                {
                    allMet = false;
                    break;
                }
            }

            if (allMet)
            {
                foreach (var req in task.requirements)
                {
                    if (req == null || req.item == null)
                        continue;

                    pool[req.item] -= req.amount;

                    req.currentAmount = inventory.GetItemCount(req.item);
                }
            }
            else
            {
                foreach (var req in task.requirements)
                {
                    if (req == null || req.item == null)
                        continue;

                    req.currentAmount = inventory.GetItemCount(req.item);
                }
            }

            task.isCompleted = allMet;
        }
    }

    
    // COMPLETION RATE
   
    public float GetCompletionRate()
    {
        int total = 0;
        int completed = 0;

        foreach (var t in tasks)
        {
            total++;

            if (t.isCompleted)
                completed++;
        }

        if (total == 0)
            return 1f;

        return (float)completed / total;
    }

 
    // CONSUME ITEMS
   
    public void ConsumeCompletedTaskItems(InventoryManager inventory)
    {
        foreach (var task in tasks)
        {
            if (!task.isCompleted)
                continue;

            if (task.requirements == null)
                continue;

            foreach (var req in task.requirements)
            {
                if (req == null || req.item == null)
                    continue;

                inventory.RemoveItem(req.item, req.amount);
            }
        }
    }

   
    // UI BREAKDOWN
    
    public string GetAllTasksBreakdown()
    {
        string result = "";

        foreach (var task in tasks)
        {
            string status = task.isCompleted ? "DONE" : "...";

            result += status + " " + task.taskName + "\n";

            if (task.requirements != null)
            {
                foreach (var req in task.requirements)
                {
                    if (req == null || req.item == null)
                        continue;

                    int owned = inventory.GetItemCount(req.item);
                    int missing = Mathf.Max(0, req.amount - owned);

                    string reqStatus = owned >= req.amount ? "DONE" : "...";

                    result += $"{reqStatus} {req.item.itemName}: {owned}/{req.amount}";

                    if (missing > 0)
                        result += $" (Missing {missing})";

                    result += "\n";
                }
            }

            result += "\n";
        }

        return result;
    }

    /// <summary>
    /// Checks if a task exists today, and outputs whether it is completed.
    /// Returns true if the task exists, false if it wasn't generated today.
    /// </summary>
    public bool TryCheckTaskStatus(string taskName, out bool isComplete)
    {
        foreach (var task in tasks)
        {
            // Match the name exactly (works for manual tasks like "Talk To Daughter")
            if (task.taskName == taskName)
            {
                isComplete = task.isCompleted;
                return true; // Task exists!
            }
        }

        isComplete = false;
        return false; // Task does not exist today
    }
}