using UnityEngine;

public class CanvasManager : MonoBehaviour
{
    [Header("All Canvases")]
    [SerializeField] private GameObject[] canvases;

    public bool inventory = false;

    private int currentCanvasIndex = -1;

    void Start()
    {
        HideAllCanvases();
        inventory = false;
    }

    /// <summary>
    /// Shows only the selected canvas by index
    /// </summary>
    public void ShowCanvas(int index)
    {
        if (index < 0 || index >= canvases.Length)
        {
            Debug.LogWarning("Canvas index out of range!");
            return;
        }

        HideAllCanvases();

        if (index == 0)
        {
            ToggleInventory();
        }
        else
        {
            canvases[index].SetActive(true);
            currentCanvasIndex = index;
            inventory = false;
        }   
    }

    public void HideCanvas(int index)
    {
        if (index < 0 || index >= canvases.Length)
        {
            Debug.LogWarning("Canvas index out of range!");
            return;
        }

        HideAllCanvases();

        canvases[index].SetActive(false);

        if (index == 0)
        {
            ToggleInventory();
        }

        currentCanvasIndex = index;
    }

    public void HideAllCanvases()
    {
        for (int i = 0; i < canvases.Length; i++)
        {
            if (canvases[i] != null)
                canvases[i].SetActive(false);
        }
    }

    public int GetCurrentCanvas()
    {
        return currentCanvasIndex;
    }

    public void ToggleInventory()
    {
        inventory = !inventory;

        canvases[0].SetActive(inventory);
    }
}