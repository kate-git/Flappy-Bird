using UnityEngine;
using System.Collections;

public class Haptics : MonoBehaviour
{
    public float jumpVibrationStrength = 1.0f;
    public float jumpVibrationDuration = 0.2f;
    public float glideVibrationStrength = 0.8f;
    public float glideVibrationDuration = 0.3f;

    public float Jump_Force = 10f;
    public float ForwardForce = 5f;
    public float IncreasedForwardForce = 10f;
    public float MoveSpeed = 5f;

    public OVRInput.Button jumpButton = OVRInput.Button.One;
    public OVRInput.Button gripButton = OVRInput.Button.PrimaryHandTrigger;

    private Rigidbody rb;
    private Animator playerAnimator;

    private bool canJump = true;
    public float jumpCooldown = 1.5f;

    public float paraglideDrag = 2f;
    private bool isFalling = false;

    private bool isParagliding = false;

    private int controllerLayerIndex; // Index of the "Controller" layer in Animator

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        
        if (!TryGetComponent<Animator>(out playerAnimator))
        {
            Debug.LogWarning("Player Animator is not assigned. Animations may not play correctly.");
        }
        else
        {
            // Get the index of the "Controller" layer
            controllerLayerIndex = playerAnimator.GetLayerIndex("Controller");

            if (controllerLayerIndex == -1)
            {
                Debug.LogError("Animator does not have a layer named 'Controller'.");
            }
            else
            {
                // Set the layer weight to activate it
                playerAnimator.SetLayerWeight(controllerLayerIndex, 1.0f);
                Debug.Log("Controller layer activated.");
            }
        }
    }

    void Update()
    {
        HandleJumpInput();
        HandleParagliding();
        UpdateAnimator();
    }

    private void HandleJumpInput()
    {
        if (canJump && (OVRInput.GetDown(jumpButton) || Input.GetKeyDown(KeyCode.Space)))
        {
            Jump();
            TriggerHapticFeedback(jumpVibrationStrength, jumpVibrationDuration);
        }
    }

    private void HandleParagliding()
    {
        isFalling = rb.linearVelocity.y < 0;

        // Check if grip button is held and player is falling
        if (OVRInput.Get(gripButton) && isFalling)
        {
            isParagliding = true;
        }
        else
        {
            isParagliding = false;
        }

        if (isParagliding)
        {
            ApplyParaglide(paraglideDrag);
            IncreaseForwardSpeedWhileGliding();
            TriggerHapticFeedback(glideVibrationStrength, glideVibrationDuration);
        }
    }

    private void UpdateAnimator()
    {
        if (playerAnimator != null)
        {
            playerAnimator.SetBool("IsJumping", !canJump);
        }
    }

    void Jump()
    {
        rb.AddForce(Vector3.up * Jump_Force, ForceMode.Impulse);
        rb.AddForce(transform.forward * ForwardForce, ForceMode.Impulse);

        if (playerAnimator != null)
        {
            // Set the IsJumping parameter to true
            playerAnimator.SetBool("IsJumping", true);
        }

        canJump = false;
        StartCoroutine(JumpCooldown());
    }

    IEnumerator JumpCooldown()
    {
        yield return new WaitForSeconds(jumpCooldown);
        canJump = true;

        if (playerAnimator != null)
        {
            // Reset the IsJumping parameter
            playerAnimator.SetBool("IsJumping", false);
        }
    }

    private void ApplyParaglide(float dragAmount)
    {
        // Reduce the falling speed by applying drag
        Vector3 dragForce = new Vector3(0, rb.linearVelocity.y, 0) * -dragAmount;
        rb.AddForce(dragForce, ForceMode.Acceleration);
    }

    private void IncreaseForwardSpeedWhileGliding()
    {
        Vector3 forwardBoost = transform.forward * IncreasedForwardForce;
        rb.AddForce(forwardBoost, ForceMode.Force);
    }

    private void TriggerHapticFeedback(float strength, float duration)
    {
        OVRInput.SetControllerVibration(strength, strength, OVRInput.Controller.LTouch);
        OVRInput.SetControllerVibration(strength, strength, OVRInput.Controller.RTouch);

        StartCoroutine(StopHapticFeedback(duration));
    }

    private IEnumerator StopHapticFeedback(float duration)
    {
        yield return new WaitForSeconds(duration);
        OVRInput.SetControllerVibration(0, 0, OVRInput.Controller.LTouch);
        OVRInput.SetControllerVibration(0, 0, OVRInput.Controller.RTouch);
    }
}
