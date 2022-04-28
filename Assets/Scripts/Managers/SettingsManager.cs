using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour
{
    public static SettingsManager Instance { get; private set; }
    public delegate void UpdateAudio(AudioSetting audioSetting, float volume);
    public UpdateAudio updateAudio;
    public delegate void AwaitingNewKeyInput(int newKeyIndex);
    public AwaitingNewKeyInput awaitingNewKeyInput;
    public delegate void UpdateKeys();
    public UpdateKeys updateKeys;

    [SerializeField] private SavedSettings _settings;

    public AudioSetting MasterVolume { get; private set; }
    [SerializeField] [Tooltip("0: Music; 1: Sound FX; 2: Beat Sounds.")] private AudioSetting[] _volumeSettings = new AudioSetting[3];
    [SerializeField] private Slider[] _volumeSliders = new Slider[4];

    private bool _isChangingKey = false;
    private int _changingKeyIndex = -1;

    [Serializable]
    public struct AudioSetting
    {
        public AudioStruct[] Structs
        {
            get => _structs;
            private set => _structs = value;
        }
        [SerializeField] private AudioStruct[] _structs;

        [Serializable]
        public struct AudioStruct
        {
            public AudioSource Source
            {
                get => _source;
                private set => _source = value;
            }
            [SerializeField] private AudioSource _source;
            public float Volume
            {
                get => _volume;
                private set => _volume = value;
            }
            [Tooltip("The original volume of this AudioSource.")] [SerializeField] private float _volume;
        }
    }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }

        for (int i = 0; i < _volumeSliders.Length; i++)
        {
            switch (i)
            {
                case 0:
                    _volumeSliders[i].value = _settings.MasterVolume;
                    break;
                case 1:
                    _volumeSliders[i].value = _settings.MusicVolume;
                    break;
                case 2:
                    _volumeSliders[i].value = _settings.BackgroundSounds;
                    break;
                case 3:
                    _volumeSliders[i].value = _settings.BeatSounds;
                    break;
            }
        }
    }

    private void Start()
    {
        updateAudio += ChangeVolume;
    }

    private void OnDestroy()
    {
        updateAudio -= ChangeVolume;
    }

    public void ChangeKey(int keyIndex)
    {
        _changingKeyIndex = keyIndex;
        _isChangingKey = true;
        awaitingNewKeyInput?.Invoke(keyIndex);
    }

    public void ChangeAudio(int audioIndex)
    {
        float volume = _volumeSliders[audioIndex].value;
        if (audioIndex == 0)
        {
            updateAudio?.Invoke(MasterVolume, volume);
        }
        else
        {
            updateAudio?.Invoke(_volumeSettings[audioIndex - 1], volume);
        }
    }

    private void Update()
    {
        if (_isChangingKey)
        {
            if(Input.GetMouseButton(0)|| Input.GetMouseButton(1))
            {
                if (_changingKeyIndex >= 0)
                {
                    _changingKeyIndex = -1;
                }
                _isChangingKey = false;
                updateKeys?.Invoke();
            }
        }
    }

    private void OnGUI()
    {
        if (_isChangingKey)
        {
            if (_changingKeyIndex >= 0)
            {
                Event currentEvent = Event.current;
                if (currentEvent.isKey)
                {
                    EnterNewKey(_changingKeyIndex, currentEvent.keyCode);
                    _changingKeyIndex = -1;
                    _isChangingKey = false;
                }
            }
        }
    }

    private void EnterNewKey(int newKeyIndex, KeyCode newKey)
    {
        if(Array.Exists(_settings.keys, x => x == newKey))
        {
            int repeatKeyIndex = Array.FindIndex(_settings.keys, x => x == newKey);
            _settings.keys[repeatKeyIndex] = _settings.keys[newKeyIndex];
        }
        _settings.keys[newKeyIndex] = newKey;
        updateKeys?.Invoke();
    }

    private void ChangeVolume(AudioSetting audioSetting, float volume)
    {
        if (audioSetting.Equals(MasterVolume))
        {
            _settings.MasterVolume = volume;
        }
        else
        {
            int audioIndex = Array.FindIndex(_volumeSettings, x => x.Equals(audioSetting));
            switch (audioIndex)
            {
                case 0:
                    _settings.MusicVolume = volume;
                    break;
                case 1:
                    _settings.BackgroundSounds = volume;
                    break;
                case 2:
                    _settings.BeatSounds = volume;
                    break;
            }
        }
    }
}
