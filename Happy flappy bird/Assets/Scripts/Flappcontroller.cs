using UnityEngine;

public class HandTracker : MonoBehaviour
{
    public Transform leftHandAnchor; // Placeholder för vänster hand
    public Transform rightHandAnchor; // Placeholder för höger hand
    public Transform headAnchor; // Referenspunkt för spelarens huvud
    public Animator playerAnimator; // Reference to the Animator

    private Vector3 previousLeftHandPosition;
    private Vector3 previousRightHandPosition;

    public float flapThreshold = 0.05f; // Minsta rörelse som räknas som flax
    public float liftForce = 1.5f; // Lyftkraft vid flax
    public float heightNeededToFlap = -0.4f;
    public float forwardForce = 1f; // Framåtkraft vid flax
    public float gravityEffect = -1f; // Gravitationseffekt när man inte flaxar

    public Rigidbody birdRigidbody; // Fågelns Rigidbody

    private bool isFlapping = false;
    private float lastFlapTime = 0f;
    public float cooldownTime = 0.15f; // Tid mellan flaxningar

    private bool isReadyToFlap = false;

    public float hapticStrength = 0.8f; // Haptic feedback strength
    public float hapticDuration = 0.1f; // Duration of the haptic feedback

    void Start()
    {
        if (leftHandAnchor == null || rightHandAnchor == null || headAnchor == null || birdRigidbody == null)
        {
            Debug.LogError("HandTracker: Missing references. Please assign all required components.");
        }

        if (leftHandAnchor != null) previousLeftHandPosition = leftHandAnchor.position;
        if (rightHandAnchor != null) previousRightHandPosition = rightHandAnchor.position;

        if (playerAnimator == null)
        {
            Debug.LogError("Player Animator is missing! Please assign an Animator component.");
        }
    }

    void Update()
    {
        if (leftHandAnchor != null && rightHandAnchor != null && headAnchor != null)
        {
            // Calculate movements for both hands relative to their previous positions
            Vector3 leftHandMovement = leftHandAnchor.position - previousLeftHandPosition;
            Vector3 rightHandMovement = rightHandAnchor.position - previousRightHandPosition;

            // Check hand height relative to the head
            float leftHandHeightRelativeToHead = leftHandAnchor.position.y - headAnchor.position.y;
            float rightHandHeightRelativeToHead = rightHandAnchor.position.y - headAnchor.position.y;

            if (!isReadyToFlap)
            {
                if (ShouldBeReadyToFlap(leftHandHeightRelativeToHead, rightHandHeightRelativeToHead))
                {
                    isReadyToFlap = true;
                }
            }
            else
            {
                if (ShouldFlap(leftHandHeightRelativeToHead, rightHandHeightRelativeToHead))
                {
                    Flap();
                    isReadyToFlap = false;
                }
            }

            // Update previous positions
            previousLeftHandPosition = leftHandAnchor.position;
            previousRightHandPosition = rightHandAnchor.position;
        }
    }

    private bool ShouldFlap(float leftHandHeightRelativeToHead, float rightHandHeightRelativeToHead)
    {
        return leftHandHeightRelativeToHead < heightNeededToFlap && rightHandHeightRelativeToHead < heightNeededToFlap;
    }

    private static bool ShouldBeReadyToFlap(float leftHandHeightRelativeToHead, float rightHandHeightRelativeToHead)
    {
        return leftHandHeightRelativeToHead > 0 && rightHandHeightRelativeToHead > 0;
    }

    void FixedUpdate()
    {
        if (!isFlapping && birdRigidbody != null)
        {
            // Apply a constant downward force (simulate gravity)
            birdRigidbody.AddForce(Vector3.up * gravityEffect, ForceMode.Acceleration);
        }
    }

    void Flap()
    {
        if (birdRigidbody == null)
        {
            Debug.LogError("No Rigidbody assigned to birdRigidbody! Cannot apply lift force.");
            return;
        }

        isFlapping = true;

        // Trigger the IsFlapping animation in the Animator
        if (playerAnimator != null)
        {
            playerAnimator.SetBool("isFlapping", true);
        }

        // Apply forces
        birdRigidbody.AddForce(Vector3.up * liftForce, ForceMode.Impulse);
        birdRigidbody.AddForce(Vector3.forward * forwardForce, ForceMode.Impulse);

        // Trigger haptic feedback
        TriggerHapticFeedback(hapticStrength, hapticDuration);

        // Automatically reset flapping after the cooldown time
        Invoke(nameof(ResetFlapping), cooldownTime);
    }

    void ResetFlapping()
    {
        isFlapping = false;

        // Reset the IsFlapping animation parameter
        if (playerAnimator != null)
        {
            playerAnimator.SetBool("isFlapping", false);
        }
    }

    void TriggerHapticFeedback(float strength, float duration)
    {
        // Trigger haptics on both controllers
        OVRInput.SetControllerVibration(strength, strength, OVRInput.Controller.LTouch);
        OVRInput.SetControllerVibration(strength, strength, OVRInput.Controller.RTouch);

        // Stop haptics after the duration
        Invoke(nameof(StopHapticFeedback), duration);
    }

    void StopHapticFeedback()
    {
        OVRInput.SetControllerVibration(0, 0, OVRInput.Controller.LTouch);
        OVRInput.SetControllerVibration(0, 0, OVRInput.Controller.RTouch);
    }
}
