using UnityEngine;
using UnityEngine.UI;

public class ChildRoomManager : MonoBehaviour
{
    [Header("Room UI")]
    [SerializeField] Image roomImage;

    [Header("Stress Room Sprites")]
    [SerializeField] Sprite[] stressRoomSprites;
    // 0 = calm room
    // 1 = slightly messy
    // 2 = stressed
    // 3 = very stressed
    // 4 = overwhelmed

    [Header("References")]
    [SerializeField] StressManager stressManager;

    void Start()
    {
        UpdateRoom();
    }

    void Update()
    {
        UpdateRoom();
    }

    void UpdateRoom()
    {
        if (stressManager == null || roomImage == null)
            return;

        int stressLevel = stressManager.stressLevel;

        // Safety clamp
        stressLevel = Mathf.Clamp(stressLevel, 0, stressRoomSprites.Length - 1);

        // Change room sprite
        roomImage.sprite = stressRoomSprites[stressLevel];
    }
}