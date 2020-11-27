using UnityEngine;

// This script manages and checks player collisions

public class PlayerCollisionCheck : MonoBehaviour
{
    // Variables
    GameMaster gm;                                      // Reference to the GameMaster script
    PlayerMovement playerMove;                          // Reference to the PlayerMovement script
    AudioManager audioManager;                          // Reference to the AudioManager script

    void Start()
    {
        gm = GameMaster.GetInstance();                  // Call this singleton
        playerMove = PlayerMovement.GetInstance();    // Get the PlayerMovement Script
        audioManager = AudioManager.GetInstance();      // Call this singleton
    }

    // Check if we collide with anything
    void OnCollisionEnter2D(Collision2D Other)
    {
        // If we collide with the floor (or a platform) we can jump again
        if (Other.collider.gameObject.tag == "floor" || Other.collider.gameObject.tag == "platform" || Other.collider.gameObject.tag == "prop")
        {
            playerMove.isGrounded = true;
            playerMove.canJump = true;
        }

        // Are we colliding with a platform?
        if (Other.collider.gameObject.tag == "platform")
        {
            // Store variable locally
            int platformID = Other.gameObject.GetComponent<PlatformID>().platformID;

            #region Play Sound
            // Play sound
            if (platformID == 1)
            {
                audioManager.PlaySound("A");                     // Specify Name of the sound between the brackets
                //Debug.Log("Played: A");
            }
            else if (platformID == 2)
            {
                audioManager.PlaySound("B");                     // Specify Name of the sound between the brackets
                //Debug.Log("Played: B");
            }
            else if (platformID == 3)
            {
                audioManager.PlaySound("C");                     // Specify Name of the sound between the brackets
                //Debug.Log("Played: C");
            }
            else if (platformID == 4)
            {
                audioManager.PlaySound("D");                     // Specify Name of the sound between the brackets
                //Debug.Log("Played: D");
            }
            else if (platformID == 5)
            {
                audioManager.PlaySound("E");                     // Specify Name of the sound between the brackets
                //Debug.Log("Played: E");
            }
            else if (platformID == 6)
            {
                audioManager.PlaySound("F");                     // Specify Name of the sound between the brackets
                //Debug.Log("Played: F");
            }
            else if (platformID == 7)
            {
                audioManager.PlaySound("G");                     // Specify Name of the sound between the brackets
                //Debug.Log("Played: G");
            }
            #endregion

            // If playerOrder isn't empty and not full
            if (gm.playerOrder.Count != 0 && gm.playerOrder.Count < gm.platformOrder.Count)
            {
                // Loop through playerOrder
                for (int i = 0; i < gm.playerOrder.Count; i++)
                {
                    if (gm.playerOrder.Contains(platformID))
                    {
                        Debug.Log("Already Stored Platform!");
                        break;
                    }
                    else
                    {
                        Debug.Log("Stored: " + platformID + " from Loop");
                        // Add ID to playerOrder
                        gm.playerOrder.Add(platformID);
                    }
                }
            }
            // If playerOrder is full
            else if (gm.playerOrder.Count >= gm.platformOrder.Count)
            {
                Debug.Log("Already stored max amount of platforms for this level!");
                return;
            }
            // If playerOrder is empty
            else if (gm.playerOrder.Count == 0)
            {
                Debug.Log("Stored: " + platformID + " from Empty");
                // Add ID to playerOrder
                gm.playerOrder.Add(platformID);
            }

            // If player hits wrong platform
            if (gm.playerOrder.Count > 0) {
                if (platformID != gm.platformOrder[gm.playerOrder.Count - 1]) {
                    Debug.Log("Wrong Platform");
                    gm.OnLose();
                }
            }
        }
    }

    // Check if we leave that collider
    void OnCollisionExit2D(Collision2D Other)
    {
        // If we aren't colliding with the floor (or a platform) we can't jump (Unless we want to incorparate a double jump.)
        if (Other.collider.gameObject.tag == "floor" || Other.collider.gameObject.tag == "platform")
        {
            playerMove.isGrounded = false;
            playerMove.canJump = false;
        }
    }

    // Check if we enter a trigger
    void OnTriggerEnter2D(Collider2D Collision)
    {
        // Check trigger tag
        if (Collision.gameObject.tag == "resetOrder")
        {
            // clear player order list & play the correct pattern again
            Debug.Log("PLAYER ORDER HAS BEEN CLEARED!");
            gm.playerOrder.Clear();
            StartCoroutine(gm.PlayCorrectNotes());
        }
    }
}