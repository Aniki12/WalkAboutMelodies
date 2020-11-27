using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

// This script manages the Game's parameters (ex. Win/Lose Conditions)

public class GameMaster : MonoBehaviour
{
    #region Singleton
    // Singleton (written by: Randy Paulus)
    private static GameMaster instance;
    public static GameMaster GetInstance() { return instance; }

    void Awake()
    {
        // Check if there isn't another instance
        if (instance != null && instance != this)
        {
            // Destroy the duplicate
            Destroy(this.gameObject);
        }
        else
        {
            // There isn't an instance set this as the instance
            instance = this;
        }
    }
    #endregion

    // Variables
    public List<int> platformOrder;                         // This is the preset list with the correct order
    public List<int> playerOrder;                           // This is the list with the player's order

    public bool isPlayingAudio;

    [Space]
    public float intervalBetweenNotes = 0.8f;               // Set interval between notes

    [Header("Reference")]
    public Text winLoseText;                                // Reference to win/lose text on endscreen
    public GameObject endScreen;                            // Reference to the endscreen

    bool runOnce = false;                                   // Prevents OnWin/OnLose functions to be run infinite

    AudioManager audioManager;                              // Reference to the AudioManager script
    PlayerMovement playerMove;                              // Reference to the PlayerMovementScript

    void Start()
    {
        audioManager = AudioManager.GetInstance();          // Call this singleton
        playerMove = PlayerMovement.GetInstance();          // Call this singleton

        isPlayingAudio = true;

        StartCoroutine( PlayCorrectNotes());                // Start a coroutine
    }

    void Update()
    {
        // Check if we have reached max number of platforms
        if (playerOrder.Count == platformOrder.Count)
        {
            // Loop throught the list
            for (int i = 0; i < playerOrder.Count; i++)
            {
                // Check if the orders match
                if (playerOrder[i] == platformOrder[i] && !runOnce)
                {
                    // If they do, player wins
                    OnWin();
                    endScreen.SetActive(true);              // Sets the endscreen active
                }
            }
        }
    }

    // Play the correct pattern at the start of the game
    public IEnumerator PlayCorrectNotes()
    {
        isPlayingAudio = true;
        // Loop through list to check which notes are present
        for (int i = 0; i < platformOrder.Count; i++)
        {
            if (platformOrder[i] == 1) { audioManager.PlaySound("A"); }                 // Play Sound A
            else if (platformOrder[i] == 2) { audioManager.PlaySound("B"); }            // Play Sound B
            else if (platformOrder[i] == 3) { audioManager.PlaySound("C"); }            // Play Sound C
            else if (platformOrder[i] == 4) { audioManager.PlaySound("D"); }            // Play Sound D
            else if (platformOrder[i] == 5) { audioManager.PlaySound("E"); }            // Play Sound E
            else if (platformOrder[i] == 6) { audioManager.PlaySound("F"); }            // Play Sound F
            else if (platformOrder[i] == 7) { audioManager.PlaySound("G"); }            // Play Sound G
            yield return new WaitForSeconds(intervalBetweenNotes);                      // Wait for specified amount of seconds before playing next note
        }
        isPlayingAudio = false;
    }

    // What happens if the player wins...
    void OnWin()
    {
        runOnce = true;
        Debug.Log("PLAYER WINS!");
        playerMove.enabled = false;                                 // Disables PlayerMovement Script Component on EndScreen
        EndScreenManager.GetInstance().WinScreen();
    }
    // What happens if the player loses...
    public void OnLose()
    {
        runOnce = true;
        Debug.Log("PLAYER LOSES!");
        playerMove.enabled = false;                                 // Disables PlayerMovement Script Component on EndScreen
        EndScreenManager.GetInstance().LoseScreen();
    }
}