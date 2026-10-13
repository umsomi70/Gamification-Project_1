using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    public static EventManager instance;

    [SerializeField] private SchoolEvent[] availableEvents;

    public List<SchoolEvent> schoolEventsToday = new List<SchoolEvent>();
    public List<SchoolEvent> periodEventsToday = new List<SchoolEvent>();

    private void Awake()
    {
        instance = this;
    }

    // --------------------------------------------
    // SCHOOL EVENTS (ONLY WEEKDAYS)
    // --------------------------------------------
    public void GenerateSchoolEvents(int difficulty)
    {
        schoolEventsToday.Clear();

        int eventCount = Mathf.Clamp(1 + (difficulty - 1) * 2, 1, 8);

        // FIX: Only add events marked as "School" to this pool
        List<SchoolEvent> pool = new List<SchoolEvent>();
        foreach (var e in availableEvents)
        {
            if (e != null && e.eventType == EventType.School)
            {
                pool.Add(e);
            }
        }

        for (int i = 0; i < eventCount && pool.Count > 0; i++)
        {
            int index = Random.Range(0, pool.Count);
            schoolEventsToday.Add(pool[index]);
            pool.RemoveAt(index);
        }
    }

    // --------------------------------------------
    // PERIOD EVENTS (EVERY DAY IF ON PERIOD)
    // --------------------------------------------
    public void GeneratePeriodEvents(int difficulty)
    {
        periodEventsToday.Clear();

        int eventCount = Mathf.Clamp(difficulty, 1, 5);

        // FIX: Only add events marked as "Period" to this pool
        List<SchoolEvent> pool = new List<SchoolEvent>();
        foreach (var e in availableEvents)
        {
            if (e != null && e.eventType == EventType.Period)
            {
                pool.Add(e);
            }
        }

        for (int i = 0; i < eventCount && pool.Count > 0; i++)
        {
            int index = Random.Range(0, pool.Count);
            periodEventsToday.Add(pool[index]);
            pool.RemoveAt(index);
        }
    }
}