using UnityEngine;
using UnityEngine.UI;

public class OffScreenIndicator : MonoBehaviour
{
    [Header("Distance Settings")]
    public float hideDistance = 4f;

    [Header("UI Settings")]
    public float edgePadding = 50f;

    private Transform target;
    private Transform player;
    
    private RectTransform rectTransform;
    private Image arrowImage;

    public void SetTarget(Transform targetTransform)
    {
        target = targetTransform;
        
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null) player = playerObj.transform;

        rectTransform = GetComponent<RectTransform>();
        arrowImage = GetComponent<Image>();

        Canvas mainCanvas = FindObjectOfType<Canvas>();
        if (mainCanvas != null)
        {
            transform.SetParent(mainCanvas.transform, false);
        }
    }

    void LateUpdate()
    {
        if (target == null || player == null) return;

        float distanceToTarget = Vector2.Distance(player.position, target.position);

        if (distanceToTarget <= hideDistance)
        {
            arrowImage.enabled = false;
            return;
        }

        arrowImage.enabled = true;

        Vector3 screenPos = Camera.main.WorldToScreenPoint(target.position);

        if (screenPos.z < 0)
        {
            screenPos *= -1;
        }

        screenPos.x = Mathf.Clamp(screenPos.x, edgePadding, Screen.width - edgePadding);
        screenPos.y = Mathf.Clamp(screenPos.y, edgePadding, Screen.height - edgePadding);
        screenPos.z = 0;

        rectTransform.position = screenPos;

        Vector3 direction = (target.position - player.position).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        
        rectTransform.rotation = Quaternion.Euler(0, 0, angle - 90f); 
    }
}