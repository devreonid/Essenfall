using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class InteractablePrompt : MonoBehaviour
{
    [Header("Interaction Settings")]
    [Tooltip("Tombol yang digunakan untuk interaksi di area ini")]
    public KeyCode interactKey = KeyCode.F;

    private bool isPlayerInRange = false;
    private bool isIconHiddenByAction = false;

    private void Update()
    {
        if (isPlayerInRange && Input.GetKeyDown(interactKey))
        {
            isIconHiddenByAction = !isIconHiddenByAction;

            if (InteractionIconManager.Instance != null)
            {
                if (isIconHiddenByAction)
                {
                    InteractionIconManager.Instance.HideIcon();
                }
                else
                {
                    InteractionIconManager.Instance.ShowIcon(transform);
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isPlayerInRange = true;
            isIconHiddenByAction = false;

            if (InteractionIconManager.Instance != null)
            {
                InteractionIconManager.Instance.ShowIcon(transform);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isPlayerInRange = false;
            isIconHiddenByAction = false;

            if (InteractionIconManager.Instance != null)
            {
                InteractionIconManager.Instance.HideIcon();
            }
        }
    }
    
    public void ForceShowIconAgain()
    {
        if (isPlayerInRange && InteractionIconManager.Instance != null)
        {
            isIconHiddenByAction = false;
            InteractionIconManager.Instance.ShowIcon(transform);
        }
    }
}