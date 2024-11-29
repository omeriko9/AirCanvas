
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BellSoundGenerator : MonoBehaviour
{
    [HideInInspector]
    public AudioSource audioSourcePrefab; // Prefab for an AudioSource
    private List<AudioSource> audioSources = new List<AudioSource>();
    private int sampleRate = 44100;

    // Minor scale frequencies (A minor: A, B, C, D, E, F, G)
    public float[] minorScaleFrequencies = { 440f, 493.88f, 523.25f, 587.33f, 659.25f, 698.46f, 783.99f };

    private bool isInitialized = false;

    public void Setup(AudioSource audioSourcePrefab)
    {
        if (audioSourcePrefab == null)
        {
            Debug.LogError("BellSoundGenerator: audioSourcePrefab cannot be null during Setup!");
            return;
        }

        this.audioSourcePrefab = audioSourcePrefab;
        isInitialized = true;

        Debug.Log("BellSoundGenerator: Successfully initialized!");

        // Initialize a pool of AudioSources
        for (int i = 0; i < 10; i++) // Arbitrary pool size
        {
            AudioSource newSource = Instantiate(audioSourcePrefab, transform);
            audioSources.Add(newSource);
            newSource.playOnAwake = false;
        }
    }


    public void PlayBellSound(int scaleIndex)
    {
        if (scaleIndex < 0 || scaleIndex >= minorScaleFrequencies.Length)
        {
            Debug.LogWarning("Scale index out of range!");
            return;
        }

        float frequency = minorScaleFrequencies[scaleIndex];
        StartCoroutine(GenerateAndPlayBellSound(frequency));
    }

    private IEnumerator GenerateAndPlayBellSound(float frequency)
    {
        int sampleLength = sampleRate; // 1 second of sound
        float[] bellData = new float[sampleLength];

        for (int i = 0; i < sampleLength; i++)
        {
            float time = i / (float)sampleRate;
            bellData[i] = Mathf.Sin(2 * Mathf.PI * frequency * time) * Mathf.Exp(-3 * time);
            if (i % 1024 == 0) yield return null; // Yield for async processing
        }

        // Normalize the data
        float max = Mathf.Max(Mathf.Abs(Mathf.Min(bellData)), Mathf.Max(bellData));
        for (int i = 0; i < bellData.Length; i++)
        {
            bellData[i] /= max;
        }

        // Create and play the AudioClip
        AudioClip bellClip = AudioClip.Create($"Bell_{frequency}", sampleLength, 1, sampleRate, false);
        bellClip.SetData(bellData, 0);

        // Find an available AudioSource to play the sound
        AudioSource availableSource = audioSources.Find(source => !source.isPlaying);
        if (availableSource == null)
        {
            Debug.LogWarning("No available AudioSources in the pool!");
            yield break;
        }

        // Randomize the volume
        availableSource.volume = Random.Range(0.2f, 0.6f); // Set volume between 50% and 100%

        availableSource.clip = bellClip;
        availableSource.Play();
    }
    public void StopBellSound(float fadeOutDuration = 4f)
    {
        StartCoroutine(StopAllBellSounds(fadeOutDuration));
    }

    private IEnumerator StopAllBellSounds(float fadeOutDuration)
    {
        foreach (AudioSource source in audioSources)
        {
            if (source.isPlaying)
            {
                float startVolume = source.volume;

                // Gradually reduce volume for fade-out
                while (source.volume > 0)
                {
                    source.volume -= startVolume * Time.deltaTime / fadeOutDuration;
                    yield return null;
                }

                source.Stop();
                source.volume = startVolume; // Reset volume for future use
            }
        }
    }

}
