using System.Collections.Generic;
using UnityEngine;

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

    public void PlaySound(string soundName, float pitch = 1f)
    {
        if (soundDictionary.TryGetValue(soundName, out Sound sound))
        {
            GameObject soundObject = new GameObject("Sound");
            AudioSource audioSource = soundObject.AddComponent<AudioSource>();

            audioSource.spatialBlend = 0f;
            audioSource.rolloffMode = AudioRolloffMode.Linear;
            audioSource.minDistance = 1f;
            audioSource.maxDistance = 500f;

            audioSource.clip = sound.clip;
            audioSource.volume = sound.volume;
            audioSource.pitch = pitch;
            audioSource.Play();

            Destroy(soundObject, sound.clip.length);
        }
        else
        {
            Debug.LogWarning($"Sound {soundName} not found!");
        }
    }

    public void PlaySound(AudioClip clip, float pitch = 1f)
    {
        GameObject soundObject = new GameObject("Sound");
        AudioSource audioSource = soundObject.AddComponent<AudioSource>();
        
        audioSource.spatialBlend = 0f;
        audioSource.rolloffMode = AudioRolloffMode.Linear;
        audioSource.minDistance = 1f;
        audioSource.maxDistance = 500f;

        audioSource.clip = clip;
        audioSource.pitch = pitch;
        audioSource.Play();
    }
    
    
    public void PlaySoundRandomPitch(string soundName, float minPitch = 0.9f, float maxPitch = 1.1f)
    {
        if (soundDictionary.TryGetValue(soundName, out Sound sound))
        {
            GameObject soundObject = new GameObject("Sound");
            AudioSource audioSource = soundObject.AddComponent<AudioSource>();

            audioSource.spatialBlend = 0f;
            audioSource.pitch = Random.Range(minPitch, maxPitch);
            audioSource.clip = sound.clip;
            audioSource.volume = sound.volume;
            audioSource.Play();

            Destroy(soundObject, sound.clip.length);
        }
    }
}