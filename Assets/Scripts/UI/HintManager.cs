using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;

[System.Serializable]
public class HintSlide
{
    public string title;
    [TextArea(3, 8)]
    public string description;
}

public class HintManager : MonoBehaviour
{
    [Header("Slide Content")]
    [Tooltip("Tambahkan daftar tutorial Anda di sini")]
    public List<HintSlide> slides;
    private int currentSlideIndex = 0;

    [Header("UI References")]
    [Tooltip("Panel utama popup tutorial")]
    public GameObject popupPanel;
    
    [Tooltip("Teks judul slide (misal: Player Control)")]
    public TextMeshProUGUI titleText;
    
    [Tooltip("Teks penjelasan slide")]
    public TextMeshProUGUI descriptionText;

    [Header("Buttons")]
    public Button prevButton;
    public Button nextButton;
    public Button closeButton;

    void Start()
    {
        popupPanel.SetActive(false);

        prevButton.onClick.AddListener(ShowPreviousSlide);
        nextButton.onClick.AddListener(ShowNextSlide);
        closeButton.onClick.AddListener(ClosePopup);
    }

    public void OpenPopup()
    {
        if (slides.Count == 0) return;

        currentSlideIndex = 0;
        UpdateSlideUI();
        popupPanel.SetActive(true);
        
        Time.timeScale = 0f; 
    }

    public void ClosePopup()
    {
        popupPanel.SetActive(false);

        Time.timeScale = 1f; 
    }

    private void ShowNextSlide()
    {
        if (currentSlideIndex < slides.Count - 1)
        {
            currentSlideIndex++;
            UpdateSlideUI();
        }
    }

    private void ShowPreviousSlide()
    {
        if (currentSlideIndex > 0)
        {
            currentSlideIndex--;
            UpdateSlideUI();
        }
    }

    private void UpdateSlideUI()
    {
        HintSlide currentSlide = slides[currentSlideIndex];

        titleText.text = currentSlide.title;
        descriptionText.text = currentSlide.description;

        prevButton.interactable = (currentSlideIndex > 0);
        nextButton.interactable = (currentSlideIndex < slides.Count - 1);
    }
}