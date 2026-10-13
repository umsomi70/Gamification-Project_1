using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;

[System.Serializable]
public class ShopItemData
{
    public string itemName;
    public ItemPickup itemPickup;
    // Remnants of individual buy buttons removed to clean up your inspector!
    public TMP_Text amountText;
    public int baseCost;
    [HideInInspector] public int currentAmountToBuy;
}

[System.Serializable]
public class ItemRequirementCondition
{
    public string itemName;
    public int requiredAmount;
}

[System.Serializable]
public class BulkDiscount
{
    public string discountName = "Combo Deal";
    [Tooltip("The items and exact amounts needed in the cart to activate this discount.")]
    public List<ItemRequirementCondition> requiredCombo = new List<ItemRequirementCondition>();
    [Tooltip("How many Rands to subtract from the total bill if this combo is met.")]
    public int discountAmount;
}

public class ShopManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] InventoryManager inventory;
    [SerializeField] GameManager gameManager;
    [SerializeField] UIManager ui;
    [SerializeField] TMP_Text coinsText;
    [SerializeField] TMP_Text cartTotalText; // <-- NEW: Drag a text component here to show the current total cost!

    [Header("Auntie Flo")]
    public AuntieFlo flo;
    public GameObject ipod;

    [Header("Dynamic Shop Items")]
    [SerializeField] private List<ShopItemData> shopItems = new List<ShopItemData>();

    [Header("Bulk Discount Configurations")]
    [Tooltip("Create and customize your custom item combos and discount payouts here!")]
    [SerializeField] private List<BulkDiscount> customDiscounts = new List<BulkDiscount>();

    void Update()
    {
        // Update currency display
        coinsText.text = $"COINS: {gameManager.coins}";

        // Calculate and display the pending cart cost dynamically
        int rawTotal = CalculateRawCartTotal();
        int savedAmount = CalculateDiscountSavings();
        int finalTotal = Mathf.Max(0, rawTotal - savedAmount);

        if (cartTotalText != null)
        {
            if (savedAmount > 0)
            {
                cartTotalText.text = $"TOTAL: R{finalTotal} (Saved R{savedAmount}!)";
            }
            else
            {
                cartTotalText.text = $"TOTAL: R{finalTotal}";
            }
        }

        // Automatically update the UI labels for each item row
        foreach (var item in shopItems)
        {
            if (item.amountText != null)
            {
                int itemRowCost = item.baseCost * item.currentAmountToBuy;
                item.amountText.text = $"R{itemRowCost} ({item.currentAmountToBuy})";
            }
        }
    }

    // ==================================================
    // CART CALCULATION HELPER FUNCTIONS
    // ==================================================

    private int CalculateRawCartTotal()
    {
        int total = 0;
        foreach (var item in shopItems)
        {
            total += item.baseCost * item.currentAmountToBuy;
        }
        return total;
    }

    private int CalculateDiscountSavings()
    {
        int totalSavings = 0;

        foreach (var discount in customDiscounts)
        {
            bool comboMet = true;

            foreach (var condition in discount.requiredCombo)
            {
                ShopItemData cartItem = shopItems.Find(x => x.itemName.ToLower() == condition.itemName.ToLower());

                // If the item isn't in the cart, or they haven't queued up enough of it, the deal fails
                if (cartItem == null || cartItem.currentAmountToBuy < condition.requiredAmount)
                {
                    comboMet = false;
                    break;
                }
            }

            if (comboMet)
            {
                totalSavings += discount.discountAmount;
            }
        }

        return totalSavings;
    }

    // ==================================================
    // FIXED PARAMETER CLOGS FOR UNITY UI
    // ==================================================

    public void IncreaseItemAmount(string itemName)
    {
        ShopItemData item = shopItems.Find(x => x.itemName.ToLower() == itemName.ToLower());
        if (item != null)
        {
            item.currentAmountToBuy += 1;
        }
    }

    public void DecreaseItemAmount(string itemName)
    {
        ShopItemData item = shopItems.Find(x => x.itemName.ToLower() == itemName.ToLower());
        if (item != null)
        {
            item.currentAmountToBuy -= 1;
            if (item.currentAmountToBuy < 0)
            {
                item.currentAmountToBuy = 0;
            }
        }
    }

    // ==================================================
    // NEW CENTRALIZED CART CHECKOUT SYSTEM
    // ==================================================

    // WIRE THIS TO YOUR SINGLE MAIN CHECKOUT BUTTON
    public void CheckoutCart()
    {
        int rawTotal = CalculateRawCartTotal();

        if (rawTotal <= 0)
        {
            flo.CallAssistant("Your cart is empty!");
            return;
        }

        int savings = CalculateDiscountSavings();
        int finalCost = Mathf.Max(0, rawTotal - savings);

        if (gameManager.coins < finalCost)
        {
            flo.CallAssistant("Not enough coins to purchase cart items!");
            flo.CallAssistant($"Tip: Cart checkout requires R{finalCost}. Adjust quantities or earn more coins first!");
            return;
        }

        // Processing purchases across the entire cart selection pool
        string purchasedSummary = "Purchased: ";
        List<string> itemSummaryLines = new List<string>();

        foreach (var item in shopItems)
        {
            if (item.currentAmountToBuy > 0)
            {
                // Send directly to the game's item storage tier
                item.itemPickup.AddToStorage(item.currentAmountToBuy);
                itemSummaryLines.Add($"{item.currentAmountToBuy}x {item.itemName}");
            }
        }

        // Deduct money from economy variables
        gameManager.coins -= finalCost;

        // Formulate feedback notification message strings
        purchasedSummary += string.Join(", ", itemSummaryLines) + $"! Total: -R{finalCost}";
        if (savings > 0)
        {
            purchasedSummary += $" (Bulk Discount Saved R{savings}!)";
        }

        flo.CallAssistant(purchasedSummary);

        // Clear out item quantities in preparation for subsequent uses
        foreach (var item in shopItems)
        {
            item.currentAmountToBuy = 0;
        }
    }

    // ==================================================
    // SPECIAL ITEMS SYSTEM
    // ==================================================

    public void BuyIpod()
    {
        if (gameManager.coins < 50)
        {
            flo.CallAssistant("Not enough coins!");
            return;
        }

        ipod.SetActive(true);
        gameManager.coins -= 50;
        flo.CallAssistant("Purchased Ipod! -50");
    }
}