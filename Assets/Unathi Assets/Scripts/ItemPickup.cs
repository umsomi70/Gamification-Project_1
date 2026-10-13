using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ItemPickup : MonoBehaviour
{
    [Header("Item Data")]
    public ItemData item;

    [Header("Storage")]
    public int storedAmount = 1;

    [Header("UI")]
    public TMP_Text amountTxt;
    public Button button;

    private InventoryManager inventory;

    void Start()
    {
        inventory = FindAnyObjectByType<InventoryManager>();

        UpdateUI();
    }

    void Update()
    {
        UpdateUI();
    }

    void UpdateUI()
    {
        amountTxt.text = $"({storedAmount})";

        button.interactable = storedAmount > 0;
    }

    public void TakeFromStorage()
    {
        if (storedAmount <= 0)
            return;

        inventory.AddItem(item, 1);

        storedAmount--;
    }

    public void AddToStorage(int amount)
    {
        storedAmount += amount;
    }
}