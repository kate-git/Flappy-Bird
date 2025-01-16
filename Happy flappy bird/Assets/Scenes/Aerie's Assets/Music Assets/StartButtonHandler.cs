using UnityEngine;
using UnityEngine.UI;

public class StartButtonHandler : MonoBehaviour
{
    public Button startButton;  // Link to the UI Button that starts the music

    void Update()
    {
        // Check for the "A" button press on the Oculus controller
        if (Input.GetButtonDown("Oculus_ButtonA"))  // Change this if necessary (check input manager)
        {
            Debug.Log("A Button Pressed!"); // Debugging to ensure the button is detected
            SimulateButtonClick(); // Simulate the button click
        }
    }

    // Make this method public so it can be accessed from the UI OnClick()
    public void SimulateButtonClick()
    {
        if (startButton != null)
        {
            // Trigger the onClick event on the button
            startButton.onClick.Invoke();
        }
        else
        {
            Debug.LogWarning("Start Button not assigned!");
        }
    }
}

