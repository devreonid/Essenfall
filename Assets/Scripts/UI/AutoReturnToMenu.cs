using UnityEngine;
using System.Collections;

public class AutoReturnToMenu : MonoBehaviour
{
    [Header("Settings")]
    public string mainMenuSceneName = "MainMenu";
    public float delayBeforeReturn = 5f;

    void Start()
    {
        StartCoroutine(ReturnRoutine());
    }

    private IEnumerator ReturnRoutine()
    {
        yield return new WaitForSeconds(delayBeforeReturn);

        if (GlobalTransition.Instance != null)
        {
            GlobalTransition.Instance.FadeOutAndLoadScene(mainMenuSceneName);
        }
        else
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(mainMenuSceneName);
        }
    }
}