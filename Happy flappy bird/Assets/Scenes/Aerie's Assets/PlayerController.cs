using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public bool GameStarted = false; // Indicates whether the game has started
    public AudioSource backgroundMusic; // The AudioSource for music
    public AudioClip gameStartMusic;    // The music to play after the game starts
    public float moveSpeed = 5f;       // Speed at which the player moves forward

    void Start()
    {
        // Ensure the background music is set up
        if (backgroundMusic == null)
        {
            Debug.LogWarning("Background music AudioSource is not assigned!");
        }
    }

    void Update()
    {
        // If the game has started, move the player forward
        if (GameStarted)
        {
            transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);
        }
    }

    public void StartGame()
    {
        // Set the game as started
        GameStarted = true;

        // Change the music when the game starts
        if (backgroundMusic != null && gameStartMusic != null)
        {
            backgroundMusic.Stop();               // Stop the current music
            backgroundMusic.clip = gameStartMusic; // Assign the new music
            backgroundMusic.Play();               // Play the new music
        }

        Debug.Log("Game Started! Moving forward and playing new music.");
    }
}
