using UnityEngine;

public class RepairTrigger : MonoBehaviour
{
    [Header("Target Target")]
    [Tooltip("Tarik objek Engine atau Balloon Rig yang memiliki script PartIntegrity ke kotak ini")]
    public PartIntegrity partIntegrity;

    [Header("Mini-Game Settings")]
    [Tooltip("Masukkan Prefab UI Repair Mini Game ke sini")]

    public GameObject miniGamePrefab;
    
    private bool isPlayerInRange = false;
    private GameObject activeMiniGame;

    void Update()
    {
        if (partIntegrity == null) 
        {
            return;
        }

        if (isPlayerInRange && Input.GetKeyDown(KeyCode.F))
        {
            Debug.Log("Mencoba memperbaiki: " + gameObject.name);
            Debug.Log("HP Saat Ini: " + partIntegrity.currentHP + " / " + partIntegrity.maxHP);

            if (activeMiniGame != null)
            {
                Debug.Log("Mini-game sudah terbuka, abaikan input.");
                return;
            }

            if (partIntegrity.currentHP < partIntegrity.maxHP)
            {
                Debug.Log("Kondisi terpenuhi, memanggil OpenMiniGame()...");
                OpenMiniGame();
            }
            else
            {
                Debug.Log("Gagal buka: HP masih penuh!");
            }
            
        }
    }

    private void OpenMiniGame()
    {
        GameObject mainCanvas = GameObject.Find("Canvas");
        if (mainCanvas != null)
        {
            activeMiniGame = Instantiate(miniGamePrefab, mainCanvas.transform);
            
            activeMiniGame.GetComponent<RepairMiniGame>().StartMiniGame(partIntegrity, this.transform);
        }
        else
        {
            Debug.LogWarning("Objek bernama 'Canvas' tidak ditemukan!");
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Debug.Log("Player memasuki area trigger perbaikan");
            isPlayerInRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) 
        {
            isPlayerInRange = false;
            
            if (activeMiniGame != null) 
            {
                activeMiniGame.GetComponent<RepairMiniGame>().CloseMiniGame();
            }
        }
    }
}