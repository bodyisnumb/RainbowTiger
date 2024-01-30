using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundPlayer : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip button;
    public AudioClip battery_recharging;
    public AudioClip bush;
    public AudioClip electricity;
    public AudioClip shield_guard;
    public AudioClip success;

    private Dictionary<string, AudioClip> soundDictionary = new Dictionary<string, AudioClip>();
    private bool isPlaying = false;

    void Awake()
    {
        DontDestroyOnLoad(this);

        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        soundDictionary.Add("Button", button);
        soundDictionary.Add("Battery", battery_recharging);
        soundDictionary.Add("Bush", bush);
        soundDictionary.Add("Electricity", electricity);
        soundDictionary.Add("Shield", shield_guard);
        soundDictionary.Add("Success", success);
    }

    public void PlaySound(string soundName)
    {
        if (!isPlaying)
        {
            if (soundDictionary.ContainsKey(soundName))
            {
                audioSource.clip = soundDictionary[soundName];
                audioSource.Play();
                StartCoroutine(WaitForSound());
            }
            else
            {
                Debug.LogWarning("Sound not found: " + soundName);
            }
        }
    }

    IEnumerator WaitForSound()
    {
        isPlaying = true;
        yield return new WaitForSeconds(audioSource.clip.length);
        isPlaying = false;
    }
}


