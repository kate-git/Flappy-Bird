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

    private bool isFlapping = false;
    private float lastFlapTime = 0f;
    public float cooldownTime = 0.15f; // Tid mellan flaxningar

    private bool isReadyToFlap = false;

    void Start()
    {
        if (leftHandAnchor == null || rightHandAnchor == null || headAnchor == null || birdRigidbody == null)
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

            //Debug.Log($"Left hand height: {leftHandHeightRelativeToHead}, Right hand height: {rightHandHeightRelativeToHead}");


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

            // Tillåt rörelser både över och under huvudet
            //if (ShouldFlap(leftHandMovement, rightHandMovement, leftHandHeightRelativeToHead, rightHandHeightRelativeToHead)) // Tillåt rörelser nära huvudhöjden
            //{
            //    Flap();
            //    lastFlapTime = Time.time;
            //}

            // Uppdatera tidigare positioner
            previousLeftHandPosition = leftHandAnchor.position;
            previousRightHandPosition = rightHandAnchor.position;
        }
    }

    private bool ShouldFlap(float leftHandHeightRelativeToHead, float rightHandHeightRelativeToHead)
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            return true;
        }

        return leftHandHeightRelativeToHead < heightNeededToFlap && rightHandHeightRelativeToHead < heightNeededToFlap;
    }

    private static bool ShouldBeReadyToFlap(float leftHandHeightRelativeToHead, float rightHandHeightRelativeToHead)
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            return true;
        }

        return leftHandHeightRelativeToHead > 0 && rightHandHeightRelativeToHead > 0;
    }

    private bool ShouldFlap(Vector3 leftHandMovement, Vector3 rightHandMovement, float leftHandHeightRelativeToHead, float rightHandHeightRelativeToHead)
    {
        return Input.GetKeyDown(KeyCode.Space);

        return Time.time > lastFlapTime + cooldownTime &&
                        leftHandMovement.y > flapThreshold && rightHandMovement.y > flapThreshold &&
                        (leftHandHeightRelativeToHead > -0.2f || rightHandHeightRelativeToHead > -0.2f);
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
        if (birdRigidbody == null)
        {
            Debug.LogError("No Rigidbody assigned to birdRigidbody! Cannot apply lift force.");
            return;
        }

        isFlapping = true;

        // Applicera krafterna
        birdRigidbody.AddForce(Vector3.up * liftForce, ForceMode.Impulse);
        birdRigidbody.AddForce(Vector3.forward * forwardForce, ForceMode.Impulse);

        Debug.Log("Flap applied: Lift and forward forces");

        // Återställ flapping status efter en kort stund
        Invoke(nameof(ResetFlapping), 0.1f);
    }

    void ResetFlapping()
    {
        isFlapping = false;
    }
}
