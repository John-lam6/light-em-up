using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

public class AmbientSounds : MonoBehaviour {
    public float minDelay = 12f;
    public float maxDelay = 23f;

    public float playChance = 0.3f; // chance to play audio
    public List<AudioClip> ambienceSounds; 
    public AudioSource audioSource;
    public bool isPaused = false;

    public AudioSource humAudioSource;
    public List<AudioClip> humAudioClips;
    
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(PlayRandomSounds());
        PlayHum();
    }

    void Reset() {
        isPaused = false;
    }

    public void setPaused (bool paused) {
        isPaused = paused;
    }

    private IEnumerator PlayRandomSounds() {
        while (true) {
            float waitTime = Random.Range(minDelay, maxDelay);
            yield return new WaitForSeconds(waitTime);

            if (ambienceSounds.Count == 0 || audioSource == null) {
                continue;
            }

            if (Random.value <= playChance && !isPaused) {
                // pick an audio clip to play and the place to play the audio
                AudioClip clip = ambienceSounds[Random.Range(0, ambienceSounds.Count)];

                if (audioSource != null) {
                    audioSource.spatialBlend = 1;
                    audioSource.volume = Random.Range(0.55f, 1.0f);
                    audioSource.pitch = Random.Range(0.9f, 1.1f);
                    audioSource.PlayOneShot(clip);
                    
                    // pause the audio when the game is paused
                    if (audioSource.isPlaying && isPaused) {
                        audioSource.Pause();
                    }

                    // unpause the audio when the game is unpaused
                    if (!isPaused) {
                        audioSource.UnPause();
                    }
                }
            }
        }
    }

    private void PlayHum() {
        AudioClip clip = humAudioClips[Random.Range(0, humAudioClips.Count)];

        audioSource.volume = 0.08f;
        audioSource.PlayOneShot(clip);
        // pause the audio when the game is paused
        if (audioSource.isPlaying && isPaused) {
            audioSource.Pause();
        }
        
        // unpause the audio when the game is unpaused
        if (!isPaused) {
            audioSource.UnPause();
        }
        
    }
    
    void Update()
    {
        
    }
}
