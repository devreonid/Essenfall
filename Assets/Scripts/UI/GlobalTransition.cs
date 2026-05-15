using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class GlobalTransition : MonoBehaviour
{
    public static GlobalTransition Instance;

    [Header("Transition Settings")]
    public CanvasGroup fadeCanvasGroup;
    public float fadeDuration = 1f;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); 
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (fadeCanvasGroup != null)
        {
            fadeCanvasGroup.alpha = 1f;
            StopAllCoroutines();
            StartCoroutine(FadeRoutine(1f, 0f));
        }
    }

    public void FadeOut()
    {
        StartCoroutine(FadeRoutine(0f, 1f));
    }

    public void FadeOutAndLoadScene(string sceneName)
    {
        StartCoroutine(FadeOutAndLoadRoutine(sceneName));
    }

    private IEnumerator FadeRoutine(float startAlpha, float targetAlpha)
    {
        fadeCanvasGroup.blocksRaycasts = true;
        float elapsedTime = 0f;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            fadeCanvasGroup.alpha = Mathf.Lerp(startAlpha, targetAlpha, elapsedTime / fadeDuration);
            yield return null;
        }

        fadeCanvasGroup.alpha = targetAlpha;
        if (targetAlpha == 0f) fadeCanvasGroup.blocksRaycasts = false;
    }

    private IEnumerator FadeOutAndLoadRoutine(string sceneName)
    {
        yield return StartCoroutine(FadeRoutine(0f, 1f));
        SceneManager.LoadScene(sceneName);
    }
}