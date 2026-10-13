using UnityEngine;

public class StressManager : MonoBehaviour
{
    [Range(0, 4)]
    public int stressLevel = 0;

    public int maxStress = 4;
    public int minStress = 0;

    public void AddStress(int amount)
    {
        stressLevel += amount;
        stressLevel = Mathf.Clamp(stressLevel, minStress, maxStress);
    }

    public void ReduceStress(int amount)
    {
        stressLevel -= amount;
        stressLevel = Mathf.Clamp(stressLevel, minStress, maxStress);
    }

    public void SetStress(int value)
    {
        stressLevel = Mathf.Clamp(value, minStress, maxStress);
    }
}