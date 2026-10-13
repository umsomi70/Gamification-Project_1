using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class BadgeManager : MonoBehaviour
{
    [System.Serializable]
    public class BadgeTier
    {
        public string badgeName;
        public Image badgeImage;
        public int pointsRequired;
        [HideInInspector] public bool isUnlocked = false;
    }

    [Header("Badge Setup")]
    public List<BadgeTier> badgeTiers = new List<BadgeTier>();

    [Header("Alpha Settings")]
    [Range(0f, 1f)] public float lockedAlpha = 0.2f;
    [Range(0f, 1f)] public float unlockedAlpha = 1.0f;

    private int lastCheckedPoints = -1;
    private AuntieFlo floAssistant;

    void Start()
    {
        // Find AuntieFlo via GameManager instance safely
        if (GameManager.instance != null)
        {
            // Using a basic fallback just in case AuntieFlo isn't cached globally
            floAssistant = FindFirstObjectByType<AuntieFlo>();
        }

        // Initialize all badges to locked state visualization
        UpdateBadgeUI(0);
    }

    void Update()
    {
        // Guard check to ensure GameManager exists
        if (GameManager.instance == null) return;

        int currentPoints = GameManager.instance.caregiverPoints;

        // Only run the calculation check if points actually changed
        if (currentPoints != lastCheckedPoints)
        {
            UpdateBadgeUI(currentPoints);
            lastCheckedPoints = currentPoints;
        }
    }

    private void UpdateBadgeUI(int currentPoints)
    {
        for (int i = 0; i < badgeTiers.Count; i++)
        {
            BadgeTier tier = badgeTiers[i];

            if (tier.badgeImage == null) continue;

            // Check if player has hit the required threshold
            if (currentPoints >= tier.pointsRequired)
            {
                // Set alpha to fully visible
                SetImageAlpha(tier.badgeImage, unlockedAlpha);

                // If it was previously locked, announce the achievement!
                if (!tier.isUnlocked && Application.isPlaying && lastCheckedPoints != -1)
                {
                    tier.isUnlocked = true;
                    AnnounceUnlock(tier.badgeName, i + 1);
                }
            }
            else
            {
                // Keep it low alpha / faint
                SetImageAlpha(tier.badgeImage, lockedAlpha);
                tier.isUnlocked = false;
            }
        }
    }

    private void SetImageAlpha(Image img, float alphaValue)
    {
        Color tempColor = img.color;
        tempColor.a = alphaValue;
        img.color = tempColor;
    }

    private void AnnounceUnlock(string name, int level)
    {
        Debug.Log($"Badge Unlocked: {name} (Level {level})!");

        // Push a stylish notification to your overlay system
        AuntieFlo flo = FindFirstObjectByType<AuntieFlo>();
        if (flo != null)
        {
            flo.CallAssistant($"Unlocked Level {level}: {name}!");
        }
    }
}