using UnityEngine;

public class PlayerCollisionDetector : MonoBehaviour
{
    public EndMenuController endMenuController; // Reference to the EndMenuController script

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
        // Show the end menu
        if (endMenuController != null)
        {
            endMenuController.ShowEndMenu(); // Call the method to display the end menu
        }
        else
        {
            Debug.LogWarning("EndMenuController reference is not set!");
        }

        // Optional: Add other game over logic (e.g., stop player movement, reset game state, etc.)
    }
}

