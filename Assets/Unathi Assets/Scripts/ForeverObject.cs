using UnityEngine;

public class ForeverObject : MonoBehaviour
{
    public static ForeverObject instance;

    private void Awake()
    {
        // Check if an instance of this object already exists in the game
        if (instance != null && instance != this)
        {
            // If a copy already exists, destroy this new one immediately
            Destroy(gameObject);
            return;
        }

        // Set this object as the permanent instance
        instance = this;

        // Tell Unity not to destroy this object when loading new scenes
        DontDestroyOnLoad(gameObject);
    }
}