using UnityEngine;

public enum GameState 
{ 
    MainMenu, 
    Gameplay, 
    MinigameOverlay,
    GameOver 
}

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    
    public GameState CurrentState { get; private set; }

    private void Awake()
    {
        if (Instance == null) 
        { 
            Instance = this; 
            DontDestroyOnLoad(gameObject);
        }
        else 
        { 
            Destroy(gameObject); 
        }
    }

    private void Start()
    {
        ChangeState(GameState.Gameplay);
    }

    public void ChangeState(GameState newState)
    {
        CurrentState = newState;
        Debug.Log("State berubah menjadi: " + CurrentState);
    }
}