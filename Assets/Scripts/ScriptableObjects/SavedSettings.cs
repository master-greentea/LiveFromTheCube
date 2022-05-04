using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Saved Settings", menuName = "ScriptableObjects/New Saved Settings", order = 0)]
public class SavedSettings : ScriptableObject
{
    [Header("Controls")]
    [Tooltip("Keys 0-3 are the four Bosu keys from left to right.")]
    public KeyCode[] keys = new KeyCode[4];

    public float MasterVolume
    {
        get => _masterVolume = Mathf.Clamp(_masterVolume, 0f, 1f);
        set => _masterVolume = value;
    }
    [Header("Audio")]
    [SerializeField] [Range(0f, 1f)] private float _masterVolume;

    public float BeatSounds
    {
        get => _beatSounds = Mathf.Clamp(_beatSounds, 0f, 1f);
        set => _beatSounds = value;
    }
    [SerializeField] [Range(0f, 1f)] private float _beatSounds;

    public float BackgroundSounds
    {
        get => _backgroundSounds = Mathf.Clamp(_backgroundSounds, 0f, 1f);
        set => _backgroundSounds = value;
    }
    [SerializeField] [Range(0f, 1f)] private float _backgroundSounds;

    public float MusicVolume
    {
        get => _musicVolume = Mathf.Clamp(_musicVolume, 0f, 1f);
        set => _musicVolume = value;
    }
    [SerializeField] [Range(0f, 1f)] private float _musicVolume;
}
