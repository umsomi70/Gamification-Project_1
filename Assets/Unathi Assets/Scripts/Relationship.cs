using UnityEngine;

public class Relationship : MonoBehaviour
{
    [Range(0, 4)]
    public int relationship = 0;

    public int maxRelation = 10;
    public int minRelation = 0;

    private void Start()
    {
         relationship = 0;
    }

    public void AddRelation(int amount)
    {
        relationship += amount;
        relationship = Mathf.Clamp(relationship, minRelation, maxRelation);
    }

    public void ReduceRelation(int amount)
    {
        relationship -= amount;
        relationship = Mathf.Clamp(relationship, minRelation, maxRelation);
    }

    public void SetRelation(int value)
    {
        relationship = Mathf.Clamp(value, minRelation, maxRelation);
    }
}