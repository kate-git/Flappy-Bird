using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.XR;

public class ButtonClickHandler : MonoBehaviour
{
    public Button menuButton; // Reference to the UI Button
    private bool isInteractable = false; // Tracks if the button can be interacted with
    private float delayTime = 10f; // Delay time in seconds

    void Start()
    {
        // Ensure the button is initially not interactable
        if (menuButton != null)
        {
            menuButton.interactable = false;
        }

        // Start a delayed activation
        Invoke(nameof(EnableButtonInteraction), delayTime);
    }

    void Update()
    {
        // Only check for input if the button is interactable
        if (isInteractable && (CheckOculusButtonA() || Input.GetKeyDown(KeyCode.R)))
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

    private void EnableButtonInteraction()
    {
        isInteractable = true;

        if (menuButton != null)
        {
            menuButton.interactable = true; // Enable the button visually
        }

        Debug.Log("Button is now interactable!");
    }
}
