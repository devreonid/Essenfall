using UnityEngine;

public class PanelToggleManager : MonoBehaviour
{
    [Header("UI Panels")]
    public GameObject secondaryStatusPanel;

    [Header("Input Settings")]
    public KeyCode toggleKey = KeyCode.Tab;

    void Start()
    {
        if (secondaryStatusPanel != null)
        {
            secondaryStatusPanel.SetActive(false);
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(toggleKey) && secondaryStatusPanel != null)
        {
            bool isActive = secondaryStatusPanel.activeSelf;
            secondaryStatusPanel.SetActive(!isActive);
        }
    }
}