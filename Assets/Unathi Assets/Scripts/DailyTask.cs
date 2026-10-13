using System;

[Serializable]
public class DailyTask
{
    public string taskName;

    public EventRequirement[] requirements;

    public bool isCompleted;

    // NEW
    public bool isManualTask;
    public bool manualCompleted;
}