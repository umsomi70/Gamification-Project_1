using UnityEngine;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("Game State")]
    public bool gameStarted = false;
    public bool gamePaused = false;
    public bool gameOver = false;

    [Header("Player Progress")]
    public int xPPoints = 0;
    public int caregiverPoints = 0;
    public int coins = 0;

    [Header("Difficulty")]
    public int difficultyLevel = 1;

    [Header("Statistics")]
    public int totalDaysPlayed = 0;
    public int successfulSchoolDays = 0;
    public int failedSchoolDays = 0;

    [Header("References")]
    [SerializeField] DayManager dayManager;
    [SerializeField] StressManager stressManager;
    [SerializeField] UIManager ui;
    [SerializeField] AuntieFlo flo;
    [SerializeField] Relationship relationship;
    [SerializeField] AudioSource achievementSound;

    public List<SchoolEvent> todaysEvents = new List<SchoolEvent>();

    public List<SchoolEvent> schoolEventsToday = new List<SchoolEvent>();
    public List<SchoolEvent> periodEventsToday = new List<SchoolEvent>();

    void Awake()
    {
        // Singleton setup
        if (instance == null)
        {
            instance = this;
            //DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        StartGame();
    }

    // =========================================================
    // GAME FLOW
    // =========================================================

    public void StartGame()
    {
        gameStarted = true;
        gamePaused = false;
        gameOver = false;

        Time.timeScale = 1f;

        Debug.Log("Game Started");

        coins = 100;
    }

    public void PauseGame()
    {
        gamePaused = true;
        Time.timeScale = 0f;

        Debug.Log("Game Paused");
    }

    public void ResumeGame()
    {
        gamePaused = false;
        Time.timeScale = 1f;

        Debug.Log("Game Resumed");
    }

    public void EndGame()
    {
        gameOver = true;

        Time.timeScale = 0f;

        Debug.Log("Game Over");
    }

    // =========================================================
    // POINT SYSTEM
    // =========================================================

    public void AddXPPoints(int amount)
    {
        xPPoints += amount;

        flo.CallAssistant($"+{amount} XP!");
        Debug.Log("Points: " + xPPoints);
        achievementSound.Play();
    }

    public void RemoveXPPoints(int amount)
    {
        xPPoints -= amount;

        if (xPPoints < 0)
            xPPoints = 0;

        flo.CallAssistant($"-{amount} XP!");

        Debug.Log("Points: " + xPPoints);
    }

    public void RemoveCaregiverPoints(int amount)
    {
        caregiverPoints -= amount;

        if (caregiverPoints < 0)
            caregiverPoints = 0;

        flo.CallAssistant($"-{amount} CP!");

        Debug.Log("Points: " + caregiverPoints);
    }

    // =========================================================
    // REPUTATION SYSTEM
    // =========================================================

    public void AddCaregiverPoints(int amount)
    {
        caregiverPoints += amount;

        flo.CallAssistant($"+{amount} CP!");
        caregiverPoints = Mathf.Clamp(caregiverPoints, 0, 100);

        achievementSound.Play();
    }

    public void AddCoins(int amount)
    {
            coins += amount;
        flo.CallAssistant($"+R{amount} Coins!")   ;
        achievementSound.Play();
    }

    public void AddRelation(int amount)
    {
        relationship.AddRelation(amount);
    }

    // =========================================================
    // SCHOOL RESULTS
    // =========================================================

    public void SuccessfulSchoolDay1()
    {
        successfulSchoolDays++;

        AddXPPoints(20);
        AddCaregiverPoints(3);
        AddCoins(20);
        AddRelation(3);
    }

    public void SuccessfulSchoolDay2()
    {
        successfulSchoolDays++;

        AddXPPoints(10);
        AddCaregiverPoints(1);
        AddCoins(10);
        AddRelation(1);
    }

    public void FailedSchoolDay()
    {
        failedSchoolDays++;

        RemoveXPPoints(5);
        RemoveCaregiverPoints(10);

        stressManager.AddStress(1);
    }

    // =========================================================
    // DAY TRACKING
    // =========================================================

    public void OnNewDay()
    {
        totalDaysPlayed++;

        // Example difficulty scaling
        if (totalDaysPlayed % 7 == 0)
        {
            if (difficultyLevel < 4)
            difficultyLevel++;
        }
    }

    public void SetTodaysEvents(List<SchoolEvent> events)
    {
        todaysEvents = events;
    }

    public void SetSchoolEvents(List<SchoolEvent> events)
    {
        schoolEventsToday = events;
    }

    public void SetPeriodEvents(List<SchoolEvent> events)
    {
        periodEventsToday = events;
    }

    public List<SchoolEvent> GetTodaysEvents()
    {
        return todaysEvents;
    }

    // =========================================================
    // SAVE / LOAD PLACEHOLDERS
    // =========================================================

    public void SaveGame()
    {
        Debug.Log("Game Saved");
    }

    public void LoadGame()
    {
        Debug.Log("Game Loaded");
    }
}