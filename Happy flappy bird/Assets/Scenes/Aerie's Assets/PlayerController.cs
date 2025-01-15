using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float forwardSpeed = 5f; // How fast the player moves forward
    public bool gameStarted = false; // Whether the game has started or not
    public AudioSource gameMusic; // The audio source to play music when the game starts

    void Update()
    {
        // If the game has started, move the player forward
        if (gameStarted)
        {
            // Move the player forward continuously
            transform.Translate(Vector3.forward * forwardSpeed * Time.deltaTime);
        }
    }

    // This method will be called when the player collides with the start trigger
    public void StartGame()
    {
        if (gameMusic != null)
        {
            gameMusic.Play(); // Play the music when the game starts
        }
        gameStarted = true; // Set gameStarted to true, which begins the player's forward movement
    }
}
