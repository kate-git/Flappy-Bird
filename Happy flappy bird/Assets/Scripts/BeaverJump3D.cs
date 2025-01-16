using System.Collections;
using UnityEngine;

public class BeaverJumpNoPush : MonoBehaviour
{
    [Header("Hur högt, hur ofta och hur snabbt bävern hoppar")]
    [Tooltip("Hur högt bävern ska hoppa (i världsenheter).")]
    public float jumpHeight = 2f;

    [Tooltip("Hur många sekunder mellan varje hopp.")]
    public float jumpInterval = 2f;

    [Tooltip("Hur lång tid (sekunder) själva hoppet tar (upp + ner).")]
    public float jumpDuration = 1f;

    // Interna variabler
    private Vector3 startPos;

    void Start()
    {
        // Spara ursprunglig position (där bävern står)
        startPos = transform.position;

        // Starta en loopande coroutine som hanterar hopplogiken
        StartCoroutine(JumpLoop());
    }

    private IEnumerator JumpLoop()
    {
        while (true)
        {
            // Vänta 'jumpInterval' innan nästa hopp startar
            yield return new WaitForSeconds(jumpInterval);

            // Hoppa uppåt och neråt under 'jumpDuration' sekunder
            float halfTime = jumpDuration / 2f;
            float timer = 0f;

            // 1) Hoppa upp (från startPos.y till startPos.y + jumpHeight)
            while (timer < halfTime)
            {
                timer += Time.deltaTime;
                float t = Mathf.Clamp01(timer / halfTime);
                float newY = Mathf.Lerp(startPos.y, startPos.y + jumpHeight, t);
                transform.position = new Vector3(startPos.x, newY, startPos.z);
                yield return null;
            }

            // 2) Hoppa ner (från startPos.y + jumpHeight till startPos.y)
            timer = 0f;
            while (timer < halfTime)
            {
                timer += Time.deltaTime;
                float t = Mathf.Clamp01(timer / halfTime);
                float newY = Mathf.Lerp(startPos.y + jumpHeight, startPos.y, t);
                transform.position = new Vector3(startPos.x, newY, startPos.z);
                yield return null;
            }

            // Säkerställ att bävern hamnar exakt på startpos igen
            transform.position = startPos;
        }
    }
}