using System.Collections;
using UnityEngine;

public class CycleManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] UIManager ui;
    [SerializeField] ChildManager childManager;

    [Header("Cycle Settings")]
    public int averageCycleLength;
    public int lastPeriodDay = 1;

    [Header("Prediction")]
    public int predictedNextPeriod;

    [Header("Period Tracking")]
    public bool isOnPeriod = false;
    public int periodDay = 0;
    public int periodLength = 5;

    [Header("Auntie Flo")]
    public AuntieFlo flo;

    void Start()
    {
        averageCycleLength = Random.Range(3, 6);
        CalculateNextPeriod();
    }

    public void OnNewDay(int currentDay)
    {
        if (isOnPeriod)
        {
            periodDay++;
        }

        CheckForPeriod(currentDay);
        UpdatePeriodState(currentDay);
    }

    void CalculateNextPeriod()
    {
        predictedNextPeriod = lastPeriodDay + averageCycleLength;
    }

    void CheckForPeriod(int currentDay)
    {
        if (!isOnPeriod &&
     currentDay == predictedNextPeriod - 2)
        {
            flo.CallAssistant("Period coming soon!");
        }

        if (currentDay >= predictedNextPeriod && !isOnPeriod)
        {
            flo.CallAssistant("Period Started!");

            isOnPeriod = true;
            periodDay = 1;

            // Day period will finish (5/5)
            lastPeriodDay = currentDay + (periodLength - 1);

            averageCycleLength = Random.Range(3, 6);
            CalculateNextPeriod();

            childManager.OnPeriodStarted();
        }
    }
    void UpdatePeriodState(int currentDay)
    {
        if (!isOnPeriod)
            return;

        ui.ShowMessage("Period Day: " + periodDay);

        if (periodDay > periodLength)
        {
            // Previous day was 5/5
            lastPeriodDay = currentDay - 1;

            averageCycleLength = Random.Range(3, 6);
            CalculateNextPeriod();

            EndPeriodText();

            isOnPeriod = false;
            periodDay = 0;
        }
    }

    public void EndPeriodText()
    {
        ui.ShowMessage("Cycle Ended");
    }
}