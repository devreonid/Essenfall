using UnityEngine;
using UnityEngine.UI;

public class PartHealthUI : MonoBehaviour
{
    [Header("Target Component")]
    [Tooltip("Tarik objek Engine/Cargo dari Hierarchy ke sini")]
    public PartIntegrity targetPart; 

    private Slider healthSlider;

    void Awake()
    {
        healthSlider = GetComponent<Slider>();
    }

    void OnEnable()
    {
        if (targetPart != null)
        {
            targetPart.OnHealthChanged += UpdateHealthBar;
            
            UpdateHealthBar(targetPart.currentHP, targetPart.maxHP); 
        }
    }

    void OnDisable()
    {
        if (targetPart != null)
        {
            targetPart.OnHealthChanged -= UpdateHealthBar;
        }
    }

    private void UpdateHealthBar(int currentHP, int maxHP)
    {
        if (healthSlider != null)
        {
            healthSlider.maxValue = maxHP;
            healthSlider.value = currentHP;
        }
    }
}