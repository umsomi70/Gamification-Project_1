using UnityEngine;

public class RoomManager : MonoBehaviour
{
    public GameObject[] rooms;

    void Start()
    {
        ShowRoom(0);
    }

    public void ShowRoom(int index)
    {
        for (int i = 0; i < rooms.Length; i++)
        {
            rooms[i].SetActive(i == index);
        }
    }
}