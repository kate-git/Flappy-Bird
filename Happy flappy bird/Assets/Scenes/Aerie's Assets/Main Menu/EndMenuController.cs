using UnityEngine;

public class EndMenuController : MonoBehaviour
{
    public GameObject endMenuPanel; // Assign the Panel GameObject in the Inspector
    public Transform playerHead;   // Assign the player’s head (camera) Transform here
    public float panelDistance = 2f; // Distance from the player where the panel should appear

    public void ShowEndMenu()
    {
        if (endMenuPanel != null && playerHead != null)
        {
            // Calculate the position in front of the player
            Vector3 panelPosition = playerHead.position + playerHead.forward * panelDistance;
            endMenuPanel.transform.position = panelPosition;

            // Align the panel to face the player
            endMenuPanel.transform.LookAt(playerHead);
            endMenuPanel.transform.rotation = Quaternion.Euler(0, endMenuPanel.transform.rotation.eulerAngles.y, 0);

            // Enable the panel
            endMenuPanel.SetActive(true);
        }
        else
        {
            Debug.LogWarning("End Menu Panel or Player Head not assigned!");
        }
    }
}
