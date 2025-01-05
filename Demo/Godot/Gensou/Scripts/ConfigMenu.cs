using GensouLib.Godot.Audio;
using GensouLib.Godot.Core;
using Godot;
using System;

public partial class ConfigMenu : Node
{
    [Export]
    public HSlider masterVolumeSlider;
    [Export]
    public HSlider bgmVolumeSlider;
    [Export]
    public HSlider bgsVolumeSlider;
    [Export]
    public HSlider seVolumeSlider;
    [Export]
    public HSlider voiceVolumeSlider;
    [Export]
    public HSlider autoPlaySpeedSlider;
    [Export]
    public HSlider textDisplaySpeedSlider;
    [Export]
    public OptionButton displayModeOptionButton;
    [Export]
    public Button Close;
    [Export]
    public Button Save;

    public override void _Ready()
    {
        masterVolumeSlider.Value = AudioManager.MasterVolume;
        bgmVolumeSlider.Value = AudioManager.BgmVolume;
        bgsVolumeSlider.Value = AudioManager.BgsVolume;
        seVolumeSlider.Value = AudioManager.SeVolume;
        voiceVolumeSlider.Value = AudioManager.VoiceVolume;

        float currentInterval = VisualNoveCore.AutoPlayInterval;
        float t = (float)((autoPlaySpeedSlider.MaxValue - currentInterval) / (float)(autoPlaySpeedSlider.MaxValue - autoPlaySpeedSlider.MinValue));
        autoPlaySpeedSlider.Value = Mathf.Lerp(autoPlaySpeedSlider.MinValue, autoPlaySpeedSlider.MaxValue, t);
    
        float currentSpeed = VisualNoveCore.TextDisplaySpeed;
        t = (float)(textDisplaySpeedSlider.MaxValue - currentSpeed) / (float)(textDisplaySpeedSlider.MaxValue - textDisplaySpeedSlider.MinValue);
        textDisplaySpeedSlider.Value = Mathf.Lerp(textDisplaySpeedSlider.MinValue, textDisplaySpeedSlider.MaxValue, t);

        if (DisplayServer.WindowGetMode(0) == DisplayServer.WindowMode.Fullscreen)
        {
            displayModeOptionButton.Selected = 0;
        }
        else
        {
            displayModeOptionButton.Selected = 1;
        }

        Close.Pressed += VisualNoveCore.CloseConfigUi;
        Save.Pressed += SaveLoadGame.SaveConfig;
    }

    public void SetDisplay(int index)
    {
        switch (index)
        {
            case 0:
                DisplayServer.WindowSetMode(DisplayServer.WindowMode.Fullscreen);
                break;
            case 1:
                DisplayServer.WindowSetMode(DisplayServer.WindowMode.Windowed);
                break;
        }
    }

    public void SetTextDisplaySpeed(float value)
    {
        float t = (float)((value - textDisplaySpeedSlider.MinValue) / (textDisplaySpeedSlider.MaxValue - textDisplaySpeedSlider.MinValue));
        VisualNoveCore.TextDisplaySpeed = (float)Mathf.Lerp(textDisplaySpeedSlider.MaxValue, textDisplaySpeedSlider.MinValue, t);
    }

    public void SetAutoPlaySpeed(float value)
    {
        float t = (float)((value - autoPlaySpeedSlider.MinValue) / (autoPlaySpeedSlider.MaxValue - autoPlaySpeedSlider.MinValue));
        VisualNoveCore.AutoPlayInterval = (int)Mathf.Lerp(autoPlaySpeedSlider.MaxValue, autoPlaySpeedSlider.MinValue, t);
    }

    public void SetMasterVolume(float value)
    {
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
