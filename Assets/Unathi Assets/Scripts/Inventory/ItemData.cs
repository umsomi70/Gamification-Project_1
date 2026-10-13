using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item")]
public class ItemData : ScriptableObject
{
    [Header("Basic Info")]
    public string itemName;
    public Sprite icon;

    [Header("Gameplay Effects")]
    public int stressEffect;   // negative = reduces stress
    public int comfortEffect;  // for later use

    [Header("Settings")]
    public bool isStackable = true;
}