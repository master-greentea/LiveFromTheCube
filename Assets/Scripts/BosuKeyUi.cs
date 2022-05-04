using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BosuKeyUi : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI[] _bosuKeyTexts = new TextMeshProUGUI[4];
    [SerializeField] private SettingsManager _settingsManager;
    [SerializeField] private SavedSettings _settings;

    void Start()
    {
        _settingsManager.updateKeys += UpdateKeyText;
    }

    private void UpdateKeyText()
    {
        for (int i = 0; i < _settings.keys.Length; i++)
        {
            _bosuKeyTexts[i].text = _settings.keys[i].ToString();
        }
    }
}
