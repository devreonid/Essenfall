using UnityEngine;
using UnityEngine.UI;

public class EscToMenu : MonoBehaviour
{
    [Header("Settings")]
    public string mainMenuSceneName = "MainMenu";
    public float holdDuration = 2f;

    [Header("UI Feedback (Optional)")]
    [Tooltip("Jika ada, Slider ini akan menunjukkan progress pengisian saat tombol ditahan")]
    public Slider holdProgressBar;

    private float currentHoldTime = 0f;
    private bool isTransitioning = false;

    void Start()
    {
        if (holdProgressBar != null)
        {
            holdProgressBar.gameObject.SetActive(false);
            holdProgressBar.maxValue = holdDuration;
            holdProgressBar.value = 0f;
        }
    }

    void Update()
    {
        if (isTransitioning) return;

        if (Input.GetKey(KeyCode.Escape))
        {
            currentHoldTime += Time.deltaTime;

            if (holdProgressBar != null)
            {
                holdProgressBar.gameObject.SetActive(true);
                holdProgressBar.value = currentHoldTime;
            }

            if (currentHoldTime >= holdDuration)
            {
                ReturnToMenu();
            }
        }
        else
        {
            currentHoldTime = 0f;
            if (holdProgressBar != null)
            {
                holdProgressBar.value = 0f;
                holdProgressBar.gameObject.SetActive(false);
            }
        }
    }

    private void ReturnToMenu()
    {
        isTransitioning = true;
        
        Time.timeScale = 1f;

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