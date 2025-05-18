using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

// Через этот класс проигрывуются все звуки и музыка
// Он просто спавнит AudioSource
// В инспекторе надо добавить все звуки 
// Потом проигрывать их так:
//  AudioManager.Instance.PlayAudio("имя_звука")
public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [System.Serializable]
    public class Sound
    {
        public string name;
        public AudioClip clip;
        [Range(0f, 1f)] public float volume = 1f;
    }

    [SerializeField] private List<Sound> sounds = new List<Sound>();
    private Dictionary<string, Sound> soundDictionary = new Dictionary<string, Sound>();

    private void Awake()
    {
        if (Instance == null) // Singleton
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        foreach (var sound in sounds)
        {
            soundDictionary.Add(sound.name, sound);
        }
    }

    private AudioSource CreateAudioSource(
        AudioClip clip, 
        float volume = 1f, 
        float pitch = 1f, 
        float length = 0f,
        float maxDistance = 1000f, 
        float minDistance = 0f, 
        float spatialBlend = 0f,
        Vector3 position = default
        )
    {
        GameObject soundObject = new GameObject("Audio Source");
        soundObject.transform.position = position;
        AudioSource audioSource = soundObject.AddComponent<AudioSource>();
        
        audioSource.clip = clip;
        audioSource.volume = volume;
        audioSource.pitch = pitch;
        audioSource.spatialBlend = spatialBlend;
        audioSource.maxDistance = maxDistance;
        audioSource.minDistance = minDistance;
        audioSource.spatialBlend = spatialBlend;
        audioSource.rolloffMode = spatialBlend > 0 ? AudioRolloffMode.Custom : AudioRolloffMode.Linear;
        
        audioSource.Play();
        Destroy(soundObject, length == 0 ? clip.length : length);
        return audioSource;
    }

    public void PlaySound(string soundName, float pitch = 1f)
    {
        if (soundDictionary.TryGetValue(soundName, out Sound sound))
        {
            CreateAudioSource(sound.clip, sound.volume, pitch);
        }
        else
        {
            Debug.LogWarning($"Sound {soundName} not found!");
        }
    }

    public void PlaySound(AudioClip clip, float pitch = 1f)
    {
        CreateAudioSource(clip, pitch: pitch);
    }
    
    
    public void PlaySoundRandomPitch(string soundName, float minPitch = 0.9f, float maxPitch = 1.1f)
    {
        if (soundDictionary.TryGetValue(soundName, out Sound sound))
        {
            CreateAudioSource(sound.clip, sound.volume, Random.Range(minPitch, maxPitch));
        }
        else
        {
            Debug.LogWarning($"Sound {soundName} not found!");
        }
    }

    public void PlaySoundOnPos(string soundName, Vector3 pos, float maxDistance = 1000f, float pitch = 1f)
    {
        if (soundDictionary.TryGetValue(soundName, out Sound sound))
        {
            CreateAudioSource(sound.clip, sound.volume, pitch, spatialBlend: 1f, maxDistance: maxDistance, position: pos);
        }
        else
        {
            Debug.LogWarning($"Sound {soundName} not found!");
        }
    }
    public void PlaySoundRandomPitchOnPos(string soundName, Vector3 pos, float maxDistance = 1000f, float minPitch = 0.9f, float maxPitch = 1.1f)
    {
        if (soundDictionary.TryGetValue(soundName, out Sound sound))
        {
            CreateAudioSource(sound.clip, sound.volume, Random.Range(minPitch, maxPitch), spatialBlend: 0.8f, maxDistance: maxDistance, position: pos);
        }
        else
        {
            Debug.LogWarning($"Sound {soundName} not found!");
        }
    }
}