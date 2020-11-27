using UnityEngine;

// This script manages what happens when you press escape

public class EscapeManager : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // Quits the application when build
            Application.Quit();

            // Exit out of playmode inside editor
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#endif
        }
    }
}
