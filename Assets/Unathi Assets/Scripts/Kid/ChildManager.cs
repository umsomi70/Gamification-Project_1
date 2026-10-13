using System.Collections;
using UnityEngine;

public class ChildManager : MonoBehaviour
{
    [Header("School")]
    public bool wentToSchoolToday = false;

    [Header("Child Settings")]
    public bool atHome = true;

    [SerializeField] GameObject childImage;

    [Header("References")]
    [SerializeField] InventoryManager inventory;
    [SerializeField] StressManager stressManager;

    [Header("Needs")]
    public ItemData requiredItem; // e.g. Pads

    [SerializeField] UIManager ui;

    void Update()
    {
        // Show/hide child depending on location
        childImage.SetActive(atHome);
    }

    public void OnPeriodStarted()
    {
        // If child is NOT at home (so at school)
        if (!atHome)
        {
            bool hasItem = CheckForRequiredItem();

            if (!hasItem)
            {
                Debug.Log("Child has no supplies at school! Stress maxed.");

                stressManager.SetStress(stressManager.maxStress);
            }
            else
            {
                Debug.Log("Child is prepared at school.");
            }
        }
    }

    bool CheckForRequiredItem()
    {
        foreach (var slot in inventory.slots)
        {
            if (slot.item == requiredItem && slot.quantity > 0)
            {
                return true;
            }
        }

        return false;
    }

    public void ToggleLocation()
    {
        atHome = !atHome;
        childImage.SetActive(atHome);
    }

    public void SendToSchool()
    {
        wentToSchoolToday = true;
    }

    public void ResetDailyStatus()
    {
        wentToSchoolToday = false;
    }
}