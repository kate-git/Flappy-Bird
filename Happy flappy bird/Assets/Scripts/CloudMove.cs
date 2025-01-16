using UnityEngine;

public class CloudMove : MonoBehaviour
{
    [Header("Molnets rörelse")]
    [Tooltip("Hur snabbt molnet rör sig.")]
    public float moveSpeed = 2f;

    [Tooltip("Hur långt (i enheter) molnet ska färdas från startläget åt varje håll.")]
    public float moveDistance = 5f;

    private float startX;
    private bool movingRight = true;

    void Start()
    {
        // Spara molnets ursprungliga X-position
        startX = transform.position.x;
    }

    void Update()
    {
        Vector3 pos = transform.position;

        if (movingRight)
        {
            pos.x += moveSpeed * Time.deltaTime;
            if (pos.x >= startX + moveDistance)
            {
                pos.x = startX + moveDistance;
                movingRight = false;
            }
        }
        else
        {
            pos.x -= moveSpeed * Time.deltaTime;
            if (pos.x <= startX - moveDistance)
            {
                pos.x = startX - moveDistance;
                movingRight = true;
            }
        }

        transform.position = pos;
    }
}
