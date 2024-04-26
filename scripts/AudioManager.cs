using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance; // Singleton instance
    
    public AudioSource backgroundMusicSource; // AudioSource for background music
    public AudioSource soundEffectSource; // AudioSource for sound effects

    // Add more AudioClip variables if you have multiple sound effects
    public AudioClip[] soundEffects;

    void Awake()
    {
        // Ensure only one instance of AudioManager exists

        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject); // Ensure AudioManager persists between scenes
    }

    // Play background music
    public void PlayBackgroundMusic(AudioClip musicClip)
    {
        if (backgroundMusicSource.isPlaying)
        {
            backgroundMusicSource.Stop();
        }
        backgroundMusicSource.clip = musicClip;
        backgroundMusicSource.Play();
    }

    // Play sound effect
    public void PlaySoundEffect(int soundIndex)
    {
        if (soundIndex >= 0 && soundIndex < soundEffects.Length)
        {
            soundEffectSource.PlayOneShot(soundEffects[soundIndex]);
        }
        else
        {
            Debug.LogWarning("Sound index out of range.");
        }
    }
}