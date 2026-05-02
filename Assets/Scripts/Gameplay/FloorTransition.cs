using UnityEngine;
using Cinemachine; // Wajib ditambahkan untuk memanggil Cinemachine

public class FloorTransition : MonoBehaviour
{
    private static FloorTransition activeTransition; // Static reference untuk memastikan hanya ada satu tangga aktif

    [Header("Destination Point")]
    [Tooltip("Used to determine the camera configuration according to floor level. (TargetZoomSize will be ignored if isSurface is true)")]
    public bool isSurface = true; // Apakah tangga ini untuk naik ke permukaan atau turun ke bawah
    public Transform targetDestination; // Titik tujuan teleport di lantai bawah/atas

    [Header("Camera Settings")]
    public CinemachineVirtualCamera virtualCamera; // Masukkan CM vcam1 ke sini
    public float targetZoomSize = 5f; // Orthographic size untuk lantai tujuan
    private float surfaceZoomSize;

    [Header("Interactive UI")]
    public GameObject interactionPopup; // Objek UI teks "Tekan [E]"
    public Vector3 offset = new Vector3(0, 3.0f, 0); // Offset posisi popup di atas kepala gameObject

    private bool isPlayerInRange = false;
    private GameObject player;

    void Start()
    {
        if (virtualCamera != null && isSurface)
        {
            surfaceZoomSize = virtualCamera.m_Lens.OrthographicSize; // Simpan zoom awal sebagai ukuran untuk permukaan
        }
        // Pastikan popup mati saat game baru mulai
        if (interactionPopup != null)
        {
            interactionPopup.SetActive(false);
        }
    }

    void Update()
    {
        // Jika player di area dan menekan tombol E
        if (isPlayerInRange && Input.GetKeyDown(KeyCode.E))
        {
            ExecuteTransition();
        }
    }

    void LateUpdate()
    {
        // Hanya pindahkan posisi JIKA script ini adalah yang sedang aktif
        if (activeTransition == this && interactionPopup != null && interactionPopup.activeSelf)
        {
            // 1. Konversi posisi World ke Screen
            Vector3 screenPos = Camera.main.WorldToScreenPoint(transform.position + offset);
            
            // 2. Terapkan ke UI
            if (screenPos.z > 0)
            {
                interactionPopup.transform.position = screenPos;
            }
        }
    }

    void ExecuteTransition()
    {
        if (targetDestination != null && player != null)
        {
            // 1. Teleport Player
            player.transform.position = targetDestination.position;
            
            // 2. Ubah Zoom Kamera
            if (virtualCamera != null)
            {
                if (isSurface)
                {
                    // Jika naik ke permukaan, zoom out
                    virtualCamera.m_Lens.OrthographicSize = surfaceZoomSize; 
                }
                else
                {
                    // Jika turun ke bawah, zoom in
                    virtualCamera.m_Lens.OrthographicSize = targetZoomSize;
                }
            }

            // 3. Matikan popup setelah teleport
            if (interactionPopup != null)
            {
                interactionPopup.SetActive(false);
            }
            
            isPlayerInRange = false; // Reset status
        }
        else
        {
            Debug.LogWarning("Target Destination atau Player belum ter-set!");
        }
    }

    // Mendeteksi saat player masuk ke trigger tangga
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            activeTransition = this;
            isPlayerInRange = true;
            player = collision.gameObject;
            
            if (interactionPopup != null)
            {
                interactionPopup.SetActive(true); // Munculkan tulisan "Tekan [E]"
            }
        }
    }

    // Mendeteksi saat player keluar dari trigger tangga
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isPlayerInRange = false;
            
            // Hanya matikan UI dan lepas kontrol jika diri sendiri yang sedang memegang kontrol
            if (activeTransition == this)
            {
                activeTransition = null;
                if (interactionPopup != null) interactionPopup.SetActive(false);
            }
            
            player = null;
        }
    }
}