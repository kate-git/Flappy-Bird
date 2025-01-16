using UnityEngine;

/// <summary>
/// Attach this script to the Player's GameObject.
/// It will detect collisions (not triggers) with objects tagged "Obstacle"
/// and then call GameOver().
/// </summary>
public class PlayerCollisionDetector : MonoBehaviour
{
    // Optional: if you have a GameManager or another script that handles Game Over logic,
    // you can reference it here.
    // public GameManager gameManager; 

    private void OnCollisionEnter(Collision collision)
    {
        // Check if the collided object has the tag "Obstacle"
        if (collision.gameObject.CompareTag("Obstacle"))
        {
            Debug.Log("Collided with obstacle. Game Over!");
            GameOver();
        }
    }

    void GameOver()
    {
        // Example: just log a message or do your actual game over logic.
        // if (gameManager != null)
        // {
        //     gameManager.TriggerGameOver();
        // }
        // else
        // {
        //     Debug.Log("Game Over - no GameManager referenced.");
        // }

        Debug.Log("GAME OVER logic goes here!");
    }
}