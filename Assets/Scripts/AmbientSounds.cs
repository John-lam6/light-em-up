using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

public class AmbientSounds : MonoBehaviour {
    public float minDelay = 12f;
    public float maxDelay = 23f;

    public float playChance = 0.5f; // chance to play audio
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
                    audioSource.spatialBlend = 0;
                    audioSource.volume = Random.Range(0.1f, 0.25f);
                    audioSource.pitch  = Random.Range(0.95f, 1.05f);
                    audioSource.priority = 180;
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
        if (humAudioSource == null || humAudioClips.Count == 0) return;

        AudioClip clip = humAudioClips[Random.Range(0, humAudioClips.Count)];

        humAudioSource.clip = clip;
        humAudioSource.priority = 200;
        humAudioSource.loop = true;
        humAudioSource.volume = 0.2f;
        humAudioSource.spatialBlend = 0;
        humAudioSource.Play();

        // pause the audio when the game is paused
        if (humAudioSource.isPlaying && isPaused) {
            humAudioSource.Pause();
        }
        
        // unpause the audio when the game is unpaused
        if (!isPaused) {
            humAudioSource.UnPause();
        }
    }
    
    void Update()
    {
        
    }
}