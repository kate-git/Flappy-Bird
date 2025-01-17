using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.XR;

public class ButtonClickHandler : MonoBehaviour
{
    public Button menuButton; // Reference to the UI Button

    void Update()
    {
        // Check if the "A" button on the Oculus controller or "R" key is pressed
        if (CheckOculusButtonA() || Input.GetKeyDown(KeyCode.R))
        {
            Debug.Log("A Button or R Key Pressed!");

            // Simulate a button click if the button is assigned
            if (menuButton != null)
            {
                menuButton.onClick.Invoke();
            }
            else
            {
                Debug.LogWarning("Menu Button not assigned!");
            }
        }
    }

    bool CheckOculusButtonA()
    {
        // Check if the Oculus controller "A" button is pressed
        InputDevice rightController = InputDevices.GetDeviceAtXRNode(XRNode.RightHand);
        if (rightController.TryGetFeatureValue(CommonUsages.primaryButton, out bool isPressed))
        {
            return isPressed;
        }
        return false;
    }

    public void LoadScene()
    {
        Debug.Log("Loading Scene: TestEnvironment");
        SceneManager.LoadScene("TestEnvironment");
    }
}
