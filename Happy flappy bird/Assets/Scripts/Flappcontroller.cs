using UnityEngine;

public class HandTracker : MonoBehaviour
{
    public Transform leftHandAnchor; // Placeholder för vänster hand
    public Transform rightHandAnchor; // Placeholder för höger hand
    public Transform headAnchor; // Referenspunkt för spelarens huvud

    private Vector3 previousLeftHandPosition;
    private Vector3 previousRightHandPosition;

    public float flapThreshold = 0.05f; // Minsta rörelse som räknas som flax
    public float liftForce = 1.5f; // Lyftkraft vid flax
    public float heightNeededToFlap = -0.4f;
    public float forwardForce = 1f; // Framåtkraft vid flax
    public float gravityEffect = -1f; // Gravitationseffekt när man inte flaxar

    public Rigidbody birdRigidbody; // Fågelns Rigidbody
    public Animator birdAnimator; // Animator-komponenten för fågeln
    public AudioSource audioSource; // AudioSource för ljuduppspelning
    public AudioClip flapSound; // Ljudklipp som spelas vid flax

    public float vibrationStrength = 0.5f; // Styrka för haptisk feedback
    public float vibrationDuration = 0.1f; // Varaktighet för haptisk feedback

    private bool isFlapping = false;
    private bool isReadyToFlap = false;

    void Start()
    {
        if (leftHandAnchor == null || rightHandAnchor == null || headAnchor == null || birdRigidbody == null || birdAnimator == null || audioSource == null)
        {
            Debug.LogError("HandTracker: Missing references. Please assign all required components.");
        }

        if (leftHandAnchor != null) previousLeftHandPosition = leftHandAnchor.position;
        if (rightHandAnchor != null) previousRightHandPosition = rightHandAnchor.position;
    }

    void Update()
    {
        if (leftHandAnchor != null && rightHandAnchor != null && headAnchor != null)
        {
            // Beräkna rörelser för båda händerna relativt till deras tidigare position
            Vector3 leftHandMovement = leftHandAnchor.position - previousLeftHandPosition;
            Vector3 rightHandMovement = rightHandAnchor.position - previousRightHandPosition;

            // Kontrollera handhöjden relativt till huvudet
            float leftHandHeightRelativeToHead = leftHandAnchor.position.y - headAnchor.position.y;
            float rightHandHeightRelativeToHead = rightHandAnchor.position.y - headAnchor.position.y;

            if (!isReadyToFlap)
            {
                if (ShouldBeReadyToFlap(leftHandHeightRelativeToHead, rightHandHeightRelativeToHead))
                {
                    Debug.Log("Ready to flap!");
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

            // Uppdatera tidigare positioner
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
            // Applicera en konstant nedåtriktad kraft (simulerar gravitation)
            birdRigidbody.AddForce(Vector3.up * gravityEffect, ForceMode.Acceleration);
        }
    }

    void Flap()
    {
        if (birdRigidbody == null || birdAnimator == null || audioSource == null)
        {
            Debug.LogError("Missing Rigidbody, Animator, or AudioSource! Cannot execute flap.");
            return;
        }

        isFlapping = true;

        // Sätt Animator-parametern IsJumping till true
        birdAnimator.SetBool("IsJumping", true);

        // Spela upp ljud för flax
        if (flapSound != null)
        {
            audioSource.PlayOneShot(flapSound);
        }
        else
        {
            Debug.LogWarning("Flap sound not assigned!");
        }

        // Applicera krafterna
        birdRigidbody.AddForce(Vector3.up * liftForce, ForceMode.Impulse);
        birdRigidbody.AddForce(Vector3.forward * forwardForce, ForceMode.Impulse);

        Debug.Log("Flap applied: Lift and forward forces");

        // Trigger haptic feedback
        TriggerHapticFeedback();

        // Återställ flapping status efter en kort stund
        Invoke(nameof(ResetFlapping), 0.1f);
    }

    void ResetFlapping()
    {
        isFlapping = false;

        // Sätt Animator-parametern IsJumping till false
        if (birdAnimator != null)
        {
            birdAnimator.SetBool("IsJumping", false);
        }
    }

    void TriggerHapticFeedback()
    {
        // Haptisk feedback för vänster och höger hand
        OVRInput.SetControllerVibration(vibrationStrength, vibrationStrength, OVRInput.Controller.LTouch);
        OVRInput.SetControllerVibration(vibrationStrength, vibrationStrength, OVRInput.Controller.RTouch);

        // Stoppa haptisk feedback efter varaktighet
        Invoke(nameof(StopHapticFeedback), vibrationDuration);
    }

    void StopHapticFeedback()
    {
        // Stoppa vibrationerna
        OVRInput.SetControllerVibration(0, 0, OVRInput.Controller.LTouch);
        OVRInput.SetControllerVibration(0, 0, OVRInput.Controller.RTouch);
    }
}
