using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonSceneLoader : MonoBehaviour
{
    [Header("Scene Settings")]
    [Tooltip("Ketik nama scene tujuan yang ingin dimuat saat tombol ini ditekan")]
    public string targetSceneName;

    public void LoadTargetScene()
    {
        if (string.IsNullOrEmpty(targetSceneName))
        {
            Debug.LogWarning("Nama target scene belum diisi di Inspector!");
            return;
        }

        TriggerTransition(targetSceneName);
    }

    public void RestartCurrentScene()
    {
        string currentScene = SceneManager.GetActiveScene().name;
        TriggerTransition(currentScene);
    }

    public void QuitGame()
    {
        Debug.Log("Keluar dari Game...");
        Application.Quit();
    }

    private void TriggerTransition(string sceneToLoad)
    {
        if (GlobalTransition.Instance != null)
        {
            GlobalTransition.Instance.FadeOutAndLoadScene(sceneToLoad);
        }
        else
        {
            SceneManager.LoadScene(sceneToLoad);
        }
    }
}