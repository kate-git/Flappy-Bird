using UnityEngine;
using System.Collections; // Add this for IEnumerator and coroutines

public class HapticTouchFeedback : MonoBehaviour
{
    public float hapticStrength = 0.5f; // Strength of the vibration
    public float hapticDuration = 0.2f; // Duration of the vibration

    private void OnCollisionEnter(Collision collision)
    {
        // Trigger haptic feedback when the player collides with an object
        TriggerHapticFeedback(hapticStrength, hapticDuration);
    }

    private void OnTriggerEnter(Collider other)
    {
        // Trigger haptic feedback when the player enters a trigger collider
        TriggerHapticFeedback(hapticStrength, hapticDuration);
    }

    private void TriggerHapticFeedback(float strength, float duration)
    {
        // Use both controllers for vibration
        OVRInput.SetControllerVibration(strength, strength, OVRInput.Controller.LTouch);
        OVRInput.SetControllerVibration(strength, strength, OVRInput.Controller.RTouch);

        // Stop vibration after the specified duration
        StartCoroutine(StopHapticFeedback(duration));
    }

    private IEnumerator StopHapticFeedback(float duration)
    {
        // Wait for the duration of the vibration
        yield return new WaitForSeconds(duration);

        // Stop the vibration
        OVRInput.SetControllerVibration(0, 0, OVRInput.Controller.LTouch);
        OVRInput.SetControllerVibration(0, 0, OVRInput.Controller.RTouch);
    }
}
