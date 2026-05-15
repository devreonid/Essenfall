using UnityEngine;

public class InteractionIconManager : MonoBehaviour
{
    public static InteractionIconManager Instance;

    [Header("Icon References")]
    [Tooltip("Masukkan objek UI Image Anda ke sini")]
    public GameObject iconObject; 

    [Header("Positioning")]
    [Tooltip("Jarak UI melayang di atas trigger area (dalam koordinat dunia)")]
    public Vector3 offset = new Vector3(0, 1.5f, 0);

    private Transform currentTarget;
    private RectTransform iconRectTransform;

    void Awake()
    {
        if (Instance == null) 
        {
            Instance = this;
        }
        else 
        {
            Destroy(gameObject);
            return;
        }

        if (iconObject != null)
        {
            iconRectTransform = iconObject.GetComponent<RectTransform>();
            iconObject.SetActive(false);
        }
    }

    public void ShowIcon(Transform target)
    {
        currentTarget = target;
        
        if (iconObject != null) 
        {
            iconObject.SetActive(true);
        }
    }

    public void HideIcon()
    {
        currentTarget = null;
        
        if (iconObject != null) 
        {
            iconObject.SetActive(false);
        }
    }

    void LateUpdate()
    {
        if (iconObject != null && iconObject.activeSelf && currentTarget != null && iconRectTransform != null)
        {
            Vector3 screenPos = Camera.main.WorldToScreenPoint(currentTarget.position + offset);
            
            iconRectTransform.position = screenPos;
        }
    }
}