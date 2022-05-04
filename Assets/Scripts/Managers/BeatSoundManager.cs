using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeatSoundManager : MonoBehaviour
{
    [SerializeField] private AudioClip _beatSound;
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private SavedSettings _settings;

    void Update()
    {
        foreach(KeyCode key in _settings.keys)
        {
            if (Input.GetKeyDown(key))
            {
                _audioSource.PlayOneShot(_beatSound);
            }
        }
    }
}
