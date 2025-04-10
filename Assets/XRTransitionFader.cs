using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class XRTransitionFader : MonoBehaviour
{
    [Header("Fader Settings")]
    [Tooltip("Image used for fading in/out. Must be set in the inspector.")]
    public Image fadeImage;

    // Optionally mark this to persist across scene loads
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        // Make sure the image is fully transparent on start
        SetAlpha(0f);
    }

    public void FadeToBlack(float duration)
    {
        StartCoroutine(FadeRoutine(1f, duration));
    }

    public void FadeFromBlack(float duration)
    {
        StartCoroutine(FadeRoutine(0f, duration));
    }

    private IEnumerator FadeRoutine(float targetAlpha, float duration)
    {
        float startAlpha = fadeImage.color.a;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float newAlpha = Mathf.Lerp(startAlpha, targetAlpha, elapsedTime / duration);
            SetAlpha(newAlpha);
            yield return null;
        }

        // Ensure final alpha is set
        SetAlpha(targetAlpha);
    }

    private void SetAlpha(float alpha)
    {
        if (fadeImage != null)
        {
            Color c = fadeImage.color;
            c.a = alpha;
            fadeImage.color = c;
        }
    }
}
