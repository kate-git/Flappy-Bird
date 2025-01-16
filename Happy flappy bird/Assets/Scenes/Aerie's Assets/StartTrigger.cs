using UnityEngine;

public class StartTrigger : MonoBehaviour
{
    public PlayerController playerController; // Reference to the player controller script

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Trigger Activated: Game Starting!");
            playerController.StartGame(); // Start the game and change the music
        }
    }
}
