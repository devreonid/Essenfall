using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;

public class RepairMiniGame : MonoBehaviour
{
    [Header("QTE Settings")]
    [Tooltip("Jumlah tombol yang harus ditekan")]
    public int sequenceLength = 6; 
    private KeyCode[] possibleKeys = { KeyCode.W, KeyCode.A, KeyCode.S, KeyCode.D }; 
    
    private List<KeyCode> currentSequence = new List<KeyCode>();
    private int currentIndex = 0;

    [Header("UI References")]
    [Tooltip("Objek kosong dengan Horizontal Layout Group tempat tombol berjejer")]
    public Transform keyContainer; 
    [Tooltip("Prefab UI berbentuk kotak yang memiliki komponen Image dan TextMeshPro")]
    public GameObject keyUIPrefab; 
    
    [Header("Color Feedback")]
    public Color defaultColor = Color.white;
    public Color successColor = Color.green;
    public Color failColor = Color.red;

    [Header("Positioning")]
    [Tooltip("Jarak UI melayang di atas mesin (dalam kordinat dunia)")]
    public Vector3 offset = new Vector3(0, 2f, 0);

    private Transform worldTarget;
    private RectTransform rectTransform;

    private List<Image> keyImages = new List<Image>();
    private PartIntegrity targetPart;
    private bool isInputLocked = false;
    private GameObject playerObj;

    public void StartMiniGame(PartIntegrity partToRepair, Transform targetPosition)
    {
        targetPart = partToRepair;
        worldTarget = targetPosition;
        rectTransform = GetComponent<RectTransform>();

        playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            playerObj.GetComponent<PlayerMovement>().enabled = false; 
            
            playerObj.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        }

        GenerateSequence();
    }

    private void GenerateSequence()
    {
        currentIndex = 0;
        currentSequence.Clear();

        foreach (Transform child in keyContainer) 
        { 
            Destroy(child.gameObject); 
        }
        keyImages.Clear();

        for (int i = 0; i < sequenceLength; i++)
        {
            KeyCode randomKey = possibleKeys[Random.Range(0, possibleKeys.Length)];
            currentSequence.Add(randomKey);

            GameObject keyObj = Instantiate(keyUIPrefab, keyContainer);
            
            keyObj.GetComponentInChildren<TextMeshProUGUI>().text = randomKey.ToString();
            
            Image keyImg = keyObj.GetComponent<Image>();
            keyImg.color = defaultColor;
            keyImages.Add(keyImg);
        }
    }

    void Update()
    {
        if (!Input.anyKeyDown) return;

        if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1) || Input.GetMouseButtonDown(2)) return;

        if (Input.GetKeyDown(currentSequence[currentIndex]))
        {
            keyImages[currentIndex].color = successColor;
            currentIndex++;

            if (currentIndex >= sequenceLength)
            {
                CompleteRepair();
            }
        }
        else
        {
            StartCoroutine(FailSequence());
        }
    }

    private IEnumerator FailSequence()
    {
        foreach (var img in keyImages) 
        { 
            img.color = failColor; 
        }
        
        yield return new WaitForSeconds(0.25f);
        
        GenerateSequence(); 
    }

    void LateUpdate()
    {
        if (worldTarget != null && rectTransform != null)
        {
            Vector3 screenPos = Camera.main.WorldToScreenPoint(worldTarget.position + offset);
            rectTransform.position = screenPos;
        }
    }

    private void CompleteRepair()
    {
        if (targetPart != null)
        {
            targetPart.RepairPart(50); 
            Debug.Log("Perbaikan Berhasil!");
        }

        CloseMiniGame();
    }

    public void CloseMiniGame() 
    {
        if (playerObj != null)
        {
            playerObj.GetComponent<PlayerMovement>().enabled = true; 
        }

        Destroy(gameObject);
    }
}