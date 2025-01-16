using UnityEngine;

public class BeaverMoveUpDown : MonoBehaviour
{
    [Header("Bäverns upp/ner-rörelse")]
    [Tooltip("Den nedre Y-positionen (lägst punkt).")]
    public float bottomY = 0f;

    [Tooltip("Den övre Y-positionen (högst punkt).")]
    public float topY = 5f;

    [Tooltip("Hur snabbt bävern rör sig upp/ner.")]
    public float moveSpeed = 2f;

    // Internt: om vi rör oss uppåt eller neråt
    private bool movingUp = true;

    void Start()
    {
        // Sätt bävern på bottomY om du vill börja längst ner
        // (eller lämna den på en valfri startposition)
        Vector3 startPos = transform.position;
        startPos.y = bottomY;
        transform.position = startPos;
    }

    void Update()
    {
        Vector3 pos = transform.position;

        if (movingUp)
        {
            // Rör dig uppåt
            pos.y += moveSpeed * Time.deltaTime;
            // Har vi passerat topY?
            if (pos.y >= topY)
            {
                pos.y = topY;
                movingUp = false; // börja åka neråt
            }
        }
        else
        {
            // Rör dig nedåt
            pos.y -= moveSpeed * Time.deltaTime;
            // Har vi passerat bottomY?
            if (pos.y <= bottomY)
            {
                pos.y = bottomY;
                movingUp = true; // börja åka uppåt
            }
        }

        transform.position = pos;
    }
}