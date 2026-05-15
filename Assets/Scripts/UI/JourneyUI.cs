using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class JourneyUI : MonoBehaviour
{
    [Header("Core Reference")]
    public JourneyManager journeyManager;

    [Header("UI Components")]
    public Slider progressSlider;
    public TextMeshProUGUI distanceText;

    void OnEnable()
    {
        if (journeyManager != null)
        {
            journeyManager.OnDistanceUpdated += UpdateProgressUI;
        }
    }

    void OnDisable()
    {
        if (journeyManager != null)
        {
            journeyManager.OnDistanceUpdated -= UpdateProgressUI;
        }
    }

    private void UpdateProgressUI(int currentMeter, int totalMeter)
    {
        if (progressSlider != null)
        {
            progressSlider.maxValue = totalMeter;
            progressSlider.value = currentMeter;
        }

        if (distanceText != null)
        {
            distanceText.text = currentMeter + "m / " + totalMeter + "m";
        }
    }
}