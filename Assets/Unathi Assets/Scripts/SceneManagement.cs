using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagement : MonoBehaviour
{
    // Singleton instance to allow easy access from any other script
    public static SceneManagement Instance { get; private set; }

    private void Awake()
    {
        // Ensure only one instance of SceneManagement exists
        if (Instance == null)
        {
            Instance = this;
            //DontDestroyOnLoad(gameObject); // Optional: keeps manager alive across scenes
        }
        else
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// Loads a scene by its exact string name.
    /// </summary>
    public void LoadSceneByName(string sceneName)
    {
        if (!string.IsNullOrEmpty(sceneName))
        {
            SceneManager.LoadScene(sceneName);
        }
        else
        {
            Debug.LogError("SceneManagement: Provided scene name is null or empty.");
        }
    }

    /// <summary>
    /// Loads a scene by its Build Index number.
    /// </summary>
    public void LoadSceneByIndex(int sceneIndex)
    {
        if (sceneIndex >= 0 && sceneIndex < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(sceneIndex);
        }
        else
        {
            Debug.LogError("SceneManagement: Provided build index is out of range.");
        }
    }

    /// <summary>
    /// Reloads the currently active scene (great for restarts).
    /// </summary>
    public void ReloadCurrentScene()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);
    }

    /// <summary>
    /// Closes the application. Works in standalone builds.
    /// </summary>
    public void QuitGame()
    {
        Debug.Log("SceneManagement: Quitting Game...");
        Application.Quit();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false; // Stops playback inside the Unity Editor
#endif
    }
}