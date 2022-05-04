using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SettingsScreen : MonoBehaviour
{
    [SerializeField] private SavedSettings _settings;
    [SerializeField] private TextMeshProUGUI[] _bosuKeyTexts = new TextMeshProUGUI[4];
    
    void Start()
    {
        SettingsManager.Instance.updateKeys += UpdateBosuKeyText;
        SettingsManager.Instance.awaitingNewKeyInput += ClearBosuKeyText;

        UpdateBosuKeyText();

        gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        SettingsManager.Instance.updateKeys -= UpdateBosuKeyText;
        SettingsManager.Instance.awaitingNewKeyInput -= ClearBosuKeyText;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            CloseSettings();
        }
    }

    public void CloseSettings()
    {
        SettingsManager.Instance.updateKeys?.Invoke();
        gameObject.SetActive(false);
    }

    private void ClearBosuKeyText(int keyIndex)
    {
        _bosuKeyTexts[keyIndex].text = "";
    }

    private void UpdateBosuKeyText()
    {
        for (int i = 0; i < _bosuKeyTexts.Length; i++)
        {
            _bosuKeyTexts[i].text = _settings.keys[i].ToString();
        }
    }
}
