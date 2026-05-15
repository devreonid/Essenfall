using UnityEngine;
using Cinemachine;

public class FloorTransition : MonoBehaviour
{
    [Header("Destination Point")]
    [Tooltip("Used to determine the camera configuration according to floor level. (TargetZoomSize will be ignored if isSurface is true)")]
    public bool isSurface = true;
    public Transform targetDestination;

    [Header("Camera Settings")]
    public CinemachineVirtualCamera virtualCamera;
    public float targetZoomSize = 5f;
    private float surfaceZoomSize;

    [Header("Interactive UI")]
    public GameObject interactionPopup;
    public Vector3 offset = new Vector3(0, 3.0f, 0); 

    private bool isPlayerInRange = false;
    private GameObject player;

    void Start()
    {
        if (virtualCamera != null && isSurface)
        {
            surfaceZoomSize = virtualCamera.m_Lens.OrthographicSize;
        }
        if (interactionPopup != null)
        {
            interactionPopup.SetActive(false);
        }
    }

    void Update()
    {
        if (isPlayerInRange && Input.GetKeyDown(KeyCode.F))
        {
            ExecuteTransition();
        }
    }

    void ExecuteTransition()
    {
        if (targetDestination != null && player != null)
        {
            player.transform.position = targetDestination.position;
            
            if (virtualCamera != null)
            {
                if (isSurface)
                {
                    virtualCamera.m_Lens.OrthographicSize = surfaceZoomSize; 
                }
                else
                {
                    virtualCamera.m_Lens.OrthographicSize = targetZoomSize;
                }
            }

            if (interactionPopup != null)
            {
                interactionPopup.SetActive(false);
            }
            
            isPlayerInRange = false;
        }
        else
        {
            Debug.LogWarning("Target Destination atau Player belum ter-set!");
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isPlayerInRange = true;
            player = collision.gameObject;
            
            if (interactionPopup != null)
            {
                interactionPopup.SetActive(true);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isPlayerInRange = false;
            player = null;
        }
    }
}