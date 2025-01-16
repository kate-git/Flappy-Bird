using UnityEngine;

public class OVRHeadControlledMovement : MonoBehaviour
{
    public float forwardSpeed = 5f;
    public float tiltSpeed = 2f;

    private Transform headTransform;
    private Vector3 previousPosition; // Tracks the player's previous position
    private bool hasMovedInY = false;  // Tracks if the player has moved in the Y-axis
    private bool hasMovedInYZ = false; // Tracks if the player has moved in both Y and Z axes

    void Start()
    {
        // Access the CenterEyeAnchor from the OVR Camera Rig
        headTransform = OVRManager.instance.transform.Find("TrackingSpace/CenterEyeAnchor");
        if (headTransform == null)
        {
            Debug.LogError("CenterEyeAnchor not found. Ensure you're using the OVR Camera Rig.");
        }

        // Initialize the previous position
        previousPosition = transform.position;
    }

    void Update()
    {
        if (headTransform == null) return;

        // Detect movement in Y-axis
        if (!hasMovedInY && Mathf.Abs(transform.position.y - previousPosition.y) > 0.01f)
        {
            hasMovedInY = true;
        }

        // Detect movement in both Y and Z axes simultaneously
        if (!hasMovedInYZ && Mathf.Abs(transform.position.y - previousPosition.y) > 0.01f && Mathf.Abs(transform.position.z - previousPosition.z) > 0.01f)
        {
            hasMovedInYZ = true;
        }

        // Update the previous position
        previousPosition = transform.position;

        // Forward movement
        Vector3 forwardDirection = new Vector3(headTransform.forward.x, 0, headTransform.forward.z).normalized;
        Vector3 forwardMovement = forwardSpeed * Time.deltaTime * forwardDirection;

        // Sideways movement based on head tilt (only after moving in Y-axis or in both Y and Z axes)
        Vector3 sideMovement = Vector3.zero;
        if (hasMovedInY || hasMovedInYZ) // Tilt works if player moves in Y or in both Y and Z
        {
            float headTilt = headTransform.localEulerAngles.z;
            if (headTilt > 180) headTilt -= 360; // Normalize to -180 to 180
            sideMovement = -headTilt * tiltSpeed * Time.deltaTime * Vector3.right; // Negated headTilt
        }

        // Combine movements and apply to player position
        Vector3 movement = forwardMovement + sideMovement;
        transform.position += movement;
    }
}
