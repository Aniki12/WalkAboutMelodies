using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;
using UnityEngine.UI;

public class EndScreenManager : MonoBehaviour
{
    public GameObject endScreen;
    public Button endScreenButton;
    public Text endScreenButtonText;
    public Text endScreenText;

    #region Singleton
    private static EndScreenManager instance;
    public static EndScreenManager GetInstance() { return instance; }

    void Awake() {
        // Check if there isn't another instance
        if (instance != null && instance != this) {
            // Destroy the duplicate
            Destroy(this.gameObject);
        } else {
            // There isn't an instance set this as the instance
            instance = this;
        }
    }
    #endregion

    private void Start()
    {
        endScreen.SetActive(false);
    }


    // Based on if the player wins, transform the button and end screen text. A player needs to continue to next level when he wins, and retry when he loses
    public void WinScreen()
    {
        endScreen.SetActive(true);
        endScreenText.text = "That sounded beautiful!";
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        if ((currentSceneIndex + 1) >= SceneManager.sceneCountInBuildSettings)
        {
            endScreenButtonText.text = "RETRY";
            endScreenButton.onClick.AddListener(() => { LoadLevel(0); });
        } else
        {
            endScreenButtonText.text = "NEXT LEVEL";
            endScreenButton.onClick.AddListener(() => { LoadLevel(currentSceneIndex + 1); });
        }
    }

    public void LoseScreen()
    {
        endScreen.SetActive(true);
        endScreenButtonText.text = "RETRY";
        endScreenText.text = "You hit the wrong note!";
        endScreenButton.onClick.AddListener(() => { LoadLevel(0); });
    }

    public void LoadLevel(int levelIndex)
    {
        SceneManager.LoadScene(levelIndex);
    }
}
