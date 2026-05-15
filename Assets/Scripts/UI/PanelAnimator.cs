using UnityEngine;
using System.Collections;

public class PanelAnimator : MonoBehaviour
{
    [Header("Position Settings")]
    public Vector2 hiddenPosition;
    public Vector2 visiblePosition;
    
    [Header("Animation Settings")]
    public float animationDuration = 0.5f;
    public AnimationCurve easeCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);
    public KeyCode toggleKey = KeyCode.Tab;

    private RectTransform rectTransform;
    private bool isVisible = false;
    private Coroutine activeCoroutine;

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        rectTransform.anchoredPosition = hiddenPosition;
    }

    void Update()
    {
        if (Input.GetKeyDown(toggleKey))
        {
            TogglePanel();
        }
    }

    public void TogglePanel()
    {
        isVisible = !isVisible;
        
        if (activeCoroutine != null) StopCoroutine(activeCoroutine);
        
        Vector2 target = isVisible ? visiblePosition : hiddenPosition;
        activeCoroutine = StartCoroutine(AnimatePanel(target));
    }

    IEnumerator AnimatePanel(Vector2 targetPos)
    {
        Vector2 startPos = rectTransform.anchoredPosition;
        float elapsedTime = 0;

        while (elapsedTime < animationDuration)
        {
            elapsedTime += Time.deltaTime;
            float percentage = elapsedTime / animationDuration;
            
            float curveValue = easeCurve.Evaluate(percentage);
            
            rectTransform.anchoredPosition = Vector2.Lerp(startPos, targetPos, curveValue);
            yield return null;
        }

        rectTransform.anchoredPosition = targetPos;
    }
}