using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseGame : MonoBehaviour
{
    private bool _isPaused;
    [SerializeField] private GameObject _pauseMenu;
    [SerializeField] private SettingsScreen _settingsScreen;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!_isPaused)
            {
                Pause();
            }
            else if (!_settingsScreen.gameObject.activeSelf)
            {
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
