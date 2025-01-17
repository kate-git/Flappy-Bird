using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class EndMenuHandler : MonoBehaviour
{
    public GameObject endMenu;         // Reference to the End Menu Canvas
    public TextMeshProUGUI scoreText;  // Reference to the Score Text
    private int finalScore;            // Holds the player's final score

    public void ShowEndMenu(int score)
    {
        finalScore = score;
        scoreText.text = "Score: " + finalScore.ToString();
        endMenu.SetActive(true);  // Show the End Menu
    }

    public void TryAgain()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); // Reload the current scene
    }
}

