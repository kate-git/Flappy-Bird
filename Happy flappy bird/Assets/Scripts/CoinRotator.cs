using UnityEngine;

public class CoinRotator : MonoBehaviour
{
    [Header("Rotation Speed")]
    [Tooltip("Degrees per second around the Z-axis.")]
    public float rotationSpeed = 50f;

    void Update()
    {
        // Rotate around Z-axis
        transform.Rotate(0f, 0f, rotationSpeed * Time.deltaTime);
    }
}