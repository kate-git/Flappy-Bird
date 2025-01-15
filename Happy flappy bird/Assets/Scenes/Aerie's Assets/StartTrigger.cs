using UnityEngine;

public class StartTrigger : MonoBehaviour
{
    public PlayerController playerController; // Reference to the player controller script

    // This method is called when the player enters the trigger zone
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // Make sure the player is the one colliding with the trigger
        {
            playerController.StartGame(); // Start the game when the player enters the trigger
        }
    }
}