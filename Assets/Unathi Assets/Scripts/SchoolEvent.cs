using UnityEngine;

public enum EventType
{
    School,
    Period
}

[CreateAssetMenu(menuName = "School/Event")]
public class SchoolEvent : ScriptableObject
{
    public string eventName;
    public EventType eventType; // <-- NEW: Allows code to identify the type

    [TextArea]
    public string description;

    public EventRequirement[] requirements;

    [TextArea]
    public string successText;

    [TextArea]
    public string failureText;
}