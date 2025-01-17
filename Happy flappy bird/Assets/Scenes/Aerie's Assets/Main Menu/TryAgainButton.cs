using UnityEngine;
using UnityEngine.SceneManagement;

public class TryAgainButton : MonoBehaviour
{
    // This method will be called in Update() to check the input
    void Update()
    {
        // Check if E key is pressed
        if (Input.GetKeyDown(KeyCode.E))
        {
            TryAgain();
        }

        // Check if Oculus A button is pressed (using OVRInput for VR controllers)
        if (OVRInput.GetDown(OVRInput.Button.One)) // Oculus A button
        {
            TryAgain();
        }
    }

    // The method to restart the scene when input is detected
    void TryAgain()
    {
        Debug.Log("Restarting the scene...");

        // Reload the current scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
