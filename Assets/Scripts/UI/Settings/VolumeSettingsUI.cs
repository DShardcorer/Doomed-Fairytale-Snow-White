using System;
using AudioSystem;
using GeneralManagers;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class VolumeSettingsUI : MonoBehaviour, ILifecycle<UIManager>
{
    [Header("Volume Sliders")]
    [SerializeField] private Slider masterVolumeSlider;
    [SerializeField] private Slider musicVolumeSlider;
    [SerializeField] private Slider sfxVolumeSlider;
    [SerializeField] private Slider ambienceVolumeSlider;
    [SerializeField] private Slider uiVolumeSlider;
    [SerializeField] private Slider voiceVolumeSlider;
    
    [Header("Volume Value Labels")]
    [SerializeField] private TextMeshProUGUI masterVolumeLabel;
    [SerializeField] private TextMeshProUGUI musicVolumeLabel;
    [SerializeField] private TextMeshProUGUI sfxVolumeLabel;
    [SerializeField] private TextMeshProUGUI ambienceVolumeLabel;
    [SerializeField] private TextMeshProUGUI uiVolumeLabel;
    [SerializeField] private TextMeshProUGUI voiceVolumeLabel;
    
    [Header("Other Controls")]
    [SerializeField] private Toggle muteToggle;
    
    private AudioManager _audioManager;
    private const string MasterVolKey = "MasterVolume";
    private const string MusicVolKey = "MusicVolume";
    private const string SfxVolKey = "SFXVolume";
    private const string AmbienceVolKey = "AmbienceVolume";
    private const string UIVolKey = "UIVolume";
    private const string VoiceVolKey = "VoiceVolume";
    private const string MuteKey = "AudioMuted";
    

    public void Initialize(UIManager parent)
    {
        _audioManager = GameManager.Instance.AudioManager;
        
        // Initialize UI with saved values
        LoadSettings();
        
        // Setup listeners
        masterVolumeSlider.onValueChanged.AddListener(OnMasterVolumeChanged);
        musicVolumeSlider.onValueChanged.AddListener(OnMusicVolumeChanged);
        sfxVolumeSlider.onValueChanged.AddListener(OnSFXVolumeChanged);
        ambienceVolumeSlider.onValueChanged.AddListener(OnAmbienceVolumeChanged);
        uiVolumeSlider.onValueChanged.AddListener(OnUIVolumeChanged);
        voiceVolumeSlider.onValueChanged.AddListener(OnVoiceVolumeChanged);
        muteToggle.onValueChanged.AddListener(OnMuteToggled);
    }

    public void Dispose()
    {
        // Remove listeners to prevent memory leaks
        masterVolumeSlider.onValueChanged.RemoveListener(OnMasterVolumeChanged);
        musicVolumeSlider.onValueChanged.RemoveListener(OnMusicVolumeChanged);
        sfxVolumeSlider.onValueChanged.RemoveListener(OnSFXVolumeChanged);
        ambienceVolumeSlider.onValueChanged.RemoveListener(OnAmbienceVolumeChanged);
        uiVolumeSlider.onValueChanged.RemoveListener(OnUIVolumeChanged);
        voiceVolumeSlider.onValueChanged.RemoveListener(OnVoiceVolumeChanged);
        muteToggle.onValueChanged.RemoveListener(OnMuteToggled);
    }
    
    private void OnMasterVolumeChanged(float value)
    {
        _audioManager.SetMasterVolume(value);
        masterVolumeLabel.text = Mathf.RoundToInt(value * 100).ToString();
        PlayerPrefs.SetFloat(MasterVolKey, value);
        PlayerPrefs.Save();
    }
    
    private void OnMusicVolumeChanged(float value)
    {
        _audioManager.SetMusicVolume(value);
        musicVolumeLabel.text = Mathf.RoundToInt(value * 100).ToString();
        PlayerPrefs.SetFloat(MusicVolKey, value);
        PlayerPrefs.Save();
    }
    
    private void OnSFXVolumeChanged(float value)
    {
        _audioManager.SetSFXVolume(value);
        sfxVolumeLabel.text = Mathf.RoundToInt(value * 100).ToString();
        PlayerPrefs.SetFloat(SfxVolKey, value);
        PlayerPrefs.Save();
    }
    
    private void OnAmbienceVolumeChanged(float value)
    {
        _audioManager.SetAmbienceVolume(value);
        ambienceVolumeLabel.text = Mathf.RoundToInt(value * 100).ToString();
        PlayerPrefs.SetFloat(AmbienceVolKey, value);
        PlayerPrefs.Save();
    }
    
    private void OnUIVolumeChanged(float value)
    {
        _audioManager.SetUIVolume(value);
        uiVolumeLabel.text = Mathf.RoundToInt(value * 100).ToString();
        PlayerPrefs.SetFloat(UIVolKey, value);
        PlayerPrefs.Save();
    }
    
    private void OnVoiceVolumeChanged(float value)
    {
        _audioManager.SetVoiceVolume(value);
        voiceVolumeLabel.text = Mathf.RoundToInt(value * 100).ToString();
        PlayerPrefs.SetFloat(VoiceVolKey, value);
        PlayerPrefs.Save();
    }
    
    private void OnMuteToggled(bool isMuted)
    {
        _audioManager.SetMute(isMuted);
        PlayerPrefs.SetInt(MuteKey, isMuted ? 1 : 0);
        PlayerPrefs.Save();
    }
    
    public void ResetToDefaults()
    {
        // Set default values (1.0 for all volumes, no mute)
        masterVolumeSlider.value = 1.0f;
        musicVolumeSlider.value = 1.0f;
        sfxVolumeSlider.value = 1.0f;
        ambienceVolumeSlider.value = 1.0f;
        uiVolumeSlider.value = 1.0f;
        voiceVolumeSlider.value = 1.0f;
        muteToggle.isOn = false;
    }
    
    private void LoadSettings()
    {
        // Load from PlayerPrefs with default values if not found
        masterVolumeSlider.value = PlayerPrefs.GetFloat(MasterVolKey, 1.0f);
        musicVolumeSlider.value = PlayerPrefs.GetFloat(MusicVolKey, 1.0f);
        sfxVolumeSlider.value = PlayerPrefs.GetFloat(SfxVolKey, 1.0f);
        ambienceVolumeSlider.value = PlayerPrefs.GetFloat(AmbienceVolKey, 1.0f);
        uiVolumeSlider.value = PlayerPrefs.GetFloat(UIVolKey, 1.0f);
        voiceVolumeSlider.value = PlayerPrefs.GetFloat(VoiceVolKey, 1.0f);
        muteToggle.isOn = PlayerPrefs.GetInt(MuteKey, 0) == 1;
        
        // Update labels
        masterVolumeLabel.text = Mathf.RoundToInt(masterVolumeSlider.value * 100).ToString();
        musicVolumeLabel.text = Mathf.RoundToInt(musicVolumeSlider.value * 100).ToString();
        sfxVolumeLabel.text = Mathf.RoundToInt(sfxVolumeSlider.value * 100).ToString();
        ambienceVolumeLabel.text = Mathf.RoundToInt(ambienceVolumeSlider.value * 100).ToString();
        uiVolumeLabel.text = Mathf.RoundToInt(uiVolumeSlider.value * 100).ToString();
        voiceVolumeLabel.text = Mathf.RoundToInt(voiceVolumeSlider.value * 100).ToString();
    }
    
    public void Show()
    {
        gameObject.SetActive(true);
        LoadSettings();
    }
    
    public void Hide()
    {
        gameObject.SetActive(false);
    }


}