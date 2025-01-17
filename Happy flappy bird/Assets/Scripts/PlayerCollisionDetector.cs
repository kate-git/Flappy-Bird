using UnityEngine;

public class PlayerCollisionDetector : MonoBehaviour
{
    public EndMenuHandler endMenuHandler; // Reference to the End Menu Handler
    public int playerScore;              // Current player score (should be updated during gameplay)

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Obstacle"))
        {
            Debug.Log("Collided with obstacle. Game Over!");

            // Trigger the End Menu and pass the score
            if (endMenuHandler != null)
            {
                endMenuHandler.ShowEndMenu(playerScore);
            }

            // Optionally, disable the player's controls or movement
            GetComponent<PlayerController>().enabled = false;
        }
    }
}

