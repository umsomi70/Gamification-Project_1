using System;
using UnityEngine;

[Serializable]
public class EventRequirement
{
    public ItemData item;
    public int amount;
    public int stressPenalty;

    // runtime tracking (NOT saved in ScriptableObject logic)
    [NonSerialized] public int currentAmount;
}