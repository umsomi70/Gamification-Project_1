using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class FadeManager : MonoBehaviour
{
    public Image fadeImage;
    public float fadeDuration = 1f;

    void Start()
    {
        // Start hidden
        fadeImage.gameObject.SetActive(false);
    }

    public IEnumerator FadeOut()
    {
        // Enable fade panel
        fadeImage.gameObject.SetActive(true);

        float t = 0;
        Color c = fadeImage.color;

        // Start transparent
        c.a = 0;
        fadeImage.color = c;

        while (t < fadeDuration)
        {
            t += Time.deltaTime;

            c.a = t / fadeDuration;
            fadeImage.color = c;

            yield return null;
        }

        // Ensure fully black
        c.a = 1;
        fadeImage.color = c;
    }

    public IEnumerator FadeIn()
    {
        float t = 0;
        Color c = fadeImage.color;

        // Start fully black
        c.a = 1;
        fadeImage.color = c;

        while (t < fadeDuration)
        {
            t += Time.deltaTime;

            c.a = 1 - (t / fadeDuration);
            fadeImage.color = c;

            yield return null;
        }

        // Ensure fully transparent
        c.a = 0;
        fadeImage.color = c;

        // Disable after fade finishes
        fadeImage.gameObject.SetActive(false);
    }
}