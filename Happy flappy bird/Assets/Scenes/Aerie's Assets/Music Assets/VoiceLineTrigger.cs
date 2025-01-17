using UnityEngine;

public class VoiceLineTrigger : MonoBehaviour
{
    public AudioSource audioSource; // Reference to the AudioSource
    public float delay = 4f; // Delay in seconds before the voice line starts

    void Start()
    {
        // Play the audio after the specified delay
        Invoke("PlayVoiceLine", delay);
    }

    void PlayVoiceLine()
    {
        if (audioSource != null)
        {
            audioSource.Play();
        }
        else
        {
            Debug.LogWarning("AudioSource not assigned!");
        }
    }
}

