using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RhythmGameStarter;

public class PauseGame : MonoBehaviour
{
    private bool _isPaused;
    [SerializeField] private GameObject _pauseMenu;
    [SerializeField] private SettingsScreen _settingsScreen;
    [SerializeField] private SongManager _songManager;
    private bool _wasBosuPlaying;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!_isPaused)
            {
                if (!_songManager.songPaused)
                {
                    _wasBosuPlaying = true;
                    _songManager.PauseSong();
                }
                Pause();
            }
            else if (!_settingsScreen.gameObject.activeSelf)
            {
                if (_wasBosuPlaying)
                {
                    _songManager.ResumeSong();
                    _wasBosuPlaying = false;
                }
                Unpause();
            }
        }
    }

    public void Pause()
    {
        _isPaused = true;
        _pauseMenu.SetActive(true);
        Time.timeScale = 0f;
    }

    public void Unpause()
    {
        _isPaused = false;
        _pauseMenu.SetActive(false);
        Time.timeScale = 1f;
    }
}
