using UnityEngine;

public class ButtonSound : MonoBehaviour
{
    AudioSource aud;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GameObject audioObj = GameObject.Find("ButtonAudio");

        if (audioObj != null)
        {
            aud = audioObj.GetComponent<AudioSource>();
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayAudio()
    {
        aud.Play();
    }
}
