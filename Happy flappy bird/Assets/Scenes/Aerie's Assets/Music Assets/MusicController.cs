using UnityEngine;

public class MusicController : MonoBehaviour
{
    public AudioSource musicSource;  // Reference to the audio source

    public void PlayMusic()
    {
        if (musicSource != null && !musicSource.isPlaying)
        {
            musicSource.Play();
            Debug.Log("Music Started");
        }
    }
}
