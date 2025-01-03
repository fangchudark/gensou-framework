using System.Collections;
using System.Collections.Generic;
using GensouLib.Unity.Audio;
using GensouLib.Unity.Core;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ConfigMenu : MonoBehaviour
{
    public Slider masterVolumeSlider;
    public Slider bgmVolumeSlider;
    public Slider bgsVolumeSlider;
    public Slider seVolumeSlider;
    public Slider voiceVolumeSlider;
    public Slider autoPlaySpeedSlider;
    public Slider textDisplaySpeedSlider;
    public TMP_Dropdown displayDropdown;
    public Button Close;
    public Button Save;
    void Awake()
    {
        masterVolumeSlider.value = AudioManager.MasterVolume;
        bgmVolumeSlider.value = AudioManager.BgmVolume / AudioManager.MasterVolume;
        bgsVolumeSlider.value = AudioManager.BgsVolume / AudioManager.MasterVolume;
        seVolumeSlider.value = AudioManager.SeVolume / AudioManager.MasterVolume;
        voiceVolumeSlider.value = AudioManager.VoiceVolume / AudioManager.MasterVolume;
        
        float currentInterval = VisualNoveCore.AutoPlayInterval;
        float t = (autoPlaySpeedSlider.maxValue - currentInterval) / (float)(autoPlaySpeedSlider.maxValue - autoPlaySpeedSlider.minValue);
        autoPlaySpeedSlider.value = Mathf.Lerp(autoPlaySpeedSlider.minValue, autoPlaySpeedSlider.maxValue, t);
        
        float currentSpeed = VisualNoveCore.TextDisplaySpeed;
        t = (textDisplaySpeedSlider.maxValue - currentSpeed) / (float)(textDisplaySpeedSlider.maxValue - textDisplaySpeedSlider.minValue);
        textDisplaySpeedSlider.value = Mathf.Lerp(textDisplaySpeedSlider.minValue, textDisplaySpeedSlider.maxValue, t);

        if (Screen.fullScreen)
        {
            displayDropdown.value = 0;
        }
        else if (Screen.fullScreenMode == FullScreenMode.FullScreenWindow)
        {
            displayDropdown.value = 2;
        }
        else
        {
            displayDropdown.value = 1;
        }
        masterVolumeSlider.onValueChanged.AddListener(SetMasterVolume);
        bgmVolumeSlider.onValueChanged.AddListener(SetBgmVolume);
        bgsVolumeSlider.onValueChanged.AddListener(SetBgsVolume);
        seVolumeSlider.onValueChanged.AddListener(SetSeVolume);
        voiceVolumeSlider.onValueChanged.AddListener(SetVoiceVolume);
        autoPlaySpeedSlider.onValueChanged.AddListener(SetAutoPlaySpeed);
        textDisplaySpeedSlider.onValueChanged.AddListener(SetTextDisplaySpeed);
        displayDropdown.onValueChanged.AddListener(SetDisplay);
        Close.onClick.AddListener(VisualNoveCore.CloseConfigUi);
        Save.onClick.AddListener(SaveLoadGame.SaveConfig);
    }

    public void SetDisplay(int value)
    {
        if (value == 0)
        {
            // Fullscreen
            Screen.fullScreen = true;
        }
        else if (value == 1)
        {
            // Windowed
            Screen.fullScreen = false;
        }
        else if (value == 2)
        {
            // Windowed Fullscreen
            Screen.fullScreenMode = FullScreenMode.FullScreenWindow;
        }
    }

    public void SetTextDisplaySpeed(float value)
    {
        float t = (value - textDisplaySpeedSlider.minValue) / (textDisplaySpeedSlider.maxValue - textDisplaySpeedSlider.minValue);
        VisualNoveCore.TextDisplaySpeed = Mathf.Lerp(textDisplaySpeedSlider.maxValue, textDisplaySpeedSlider.minValue, t);
    }

    public void SetAutoPlaySpeed(float value)
    {
        float t = (value - autoPlaySpeedSlider.minValue) / (autoPlaySpeedSlider.maxValue - autoPlaySpeedSlider.minValue);
        VisualNoveCore.AutoPlayInterval = (int)Mathf.Lerp(autoPlaySpeedSlider.maxValue, autoPlaySpeedSlider.minValue, t);
    }

    public void SetMasterVolume(float value)
    {
        Debug.Log("SetMasterVolume: " + value);
        AudioManager.MasterVolume = value;
    }

    public void SetBgmVolume(float value)
    {
        AudioManager.BgmVolume = value;
    }

    public void SetBgsVolume(float value)
    {
        AudioManager.BgsVolume = value;
    }

    public void SetSeVolume(float value)
    {
        AudioManager.SeVolume = value;
    }

    public void SetVoiceVolume(float value)
    {
        AudioManager.VoiceVolume = value;
    }

}
