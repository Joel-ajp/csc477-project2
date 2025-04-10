using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class XRSceneTransitionTrigger : MonoBehaviour
{
    [Header("Scene Transition Settings")]
    public string sceneToLoad;
    public float transitionDelay = 0.5f;
    public bool useFadeTransition = true;
    public float fadeDuration = 1.0f;

    [Header("Trigger Settings")]
    public string triggerTag = "Player";
    public bool triggerOnce = true;

    // Flag to prevent multiple triggers
    private bool hasTriggered = false;
    
    // Internal reference to our Image for fading
    [Header("Fade UI References")]
    [Tooltip("Drag in a black full-screen UI Image here (Canvas set to Screen Space - Overlay or Screen Space - Camera)")]
    public Image fadeOverlay;

    private void Awake()
    {
        // Make sure the fade overlay starts fully transparent
        if (fadeOverlay != null) 
            SetOverlayAlpha(0f);
    }

    private void OnTriggerEnter(Collider other)
    {
        // Bail out if already triggered
        if (hasTriggered && triggerOnce) return;

        if (!string.IsNullOrEmpty(triggerTag) && !other.CompareTag(triggerTag)) return;
        if (!IsXROrigin(other.gameObject)) return;

        hasTriggered = true;
        StartCoroutine(LoadSceneRoutine());
    }

    private IEnumerator LoadSceneRoutine()
    {
        if (transitionDelay > 0f)
            yield return new WaitForSeconds(transitionDelay);

        if (useFadeTransition && fadeOverlay != null)
        {
            yield return StartCoroutine(FadeToBlack(fadeDuration));
        }

        // Register a callback to fade from black after load
        if (useFadeTransition)
            SceneManager.sceneLoaded += OnSceneLoaded;

        SceneManager.LoadScene(sceneToLoad);
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;

        // If you're using a new instance of a fade Image in the new scene,
        // you might need to find or link it again here.
        if (fadeOverlay != null)
        {
            // Optionally ensure it's fully black if you want a consistent start
            SetOverlayAlpha(1f);
            StartCoroutine(FadeFromBlack(fadeDuration));
        }
    }

    private IEnumerator FadeToBlack(float duration)
    {
        float elapsed = 0f;
        float startAlpha = fadeOverlay.color.a;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float newAlpha = Mathf.Lerp(startAlpha, 1f, elapsed / duration);
            SetOverlayAlpha(newAlpha);
            yield return null;
        }

        SetOverlayAlpha(1f);
    }

    private IEnumerator FadeFromBlack(float duration)
    {
        float elapsed = 0f;
        float startAlpha = fadeOverlay.color.a;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float newAlpha = Mathf.Lerp(startAlpha, 0f, elapsed / duration);
            SetOverlayAlpha(newAlpha);
            yield return null;
        }

        SetOverlayAlpha(0f);
    }

    private void SetOverlayAlpha(float alpha)
    {
        Color c = fadeOverlay.color;
        c.a = alpha;
        fadeOverlay.color = c;
    }

    // Example XR check
    private bool IsXROrigin(GameObject obj)
    {
        return
            obj.GetComponentInParent<Unity.XR.CoreUtils.XROrigin>() != null ||
            obj.GetComponent<Unity.XR.CoreUtils.XROrigin>() != null ||
            obj.CompareTag("Player") ||
            obj.CompareTag("MainCamera");
    }
}
