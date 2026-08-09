using UnityEngine;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    [Header("Audio Sources")]
    [SerializeField] private AudioSource SFXAudioSource;
    [SerializeField] private AudioSource MusicAudioSource;

    [Header("Audio Clips")]
    [SerializeField] private AudioClip eatClip;
    [SerializeField] private AudioClip takeDamageClip;
    [SerializeField] private AudioClip takeItemClip;

    [Header("UI Controls (Optional)")]
    [SerializeField] private Slider SFX;
    [SerializeField] private Slider Music;

    public static float sfxVolume = 1f;
    public static float musicVolume = 1f;
    public static bool SFXIsEnable = true;
    public static bool MusicIsEnable = true;

    private float tempMusicVol;
    private float tempSfxVol;
    private bool tempMusicEnable;
    private bool tempSfxEnable;
    private void Start()
    {
        musicVolume = PlayerPrefs.GetFloat("MusicVolume", 1f);
        sfxVolume = PlayerPrefs.GetFloat("SFXVolume", 1f);
        MusicIsEnable = PlayerPrefs.GetInt("MusicEnable", 1) == 1;
        SFXIsEnable = PlayerPrefs.GetInt("SFXEnable", 1) == 1;
        if (Music != null) Music.value = musicVolume;
        if (SFX != null) SFX.value = sfxVolume;
        tempMusicVol = musicVolume;
        tempSfxVol = sfxVolume;
        tempMusicEnable = MusicIsEnable;
        tempSfxEnable = SFXIsEnable;
        ApplyVolumeToSources();
    }
    public void OnSliderChanged()
    {
        if (Music != null) tempMusicVol = Music.value;
        if (SFX != null) tempSfxVol = SFX.value;

        if (MusicAudioSource != null) MusicAudioSource.volume = tempMusicVol;
        if (SFXAudioSource != null) SFXAudioSource.volume = tempSfxVol;
    }
    public void MusicOn() { tempMusicEnable = true; }
    public void MusicOff() { tempMusicEnable = false; }
    public void SFXOn() { tempSfxEnable = true; }
    public void SFXOff() { tempSfxEnable = false; }
    public void ApplySettings()
    {
        musicVolume = tempMusicVol;
        sfxVolume = tempSfxVol;
        MusicIsEnable = tempMusicEnable;
        SFXIsEnable = tempSfxEnable;

        PlayerPrefs.SetFloat("MusicVolume", musicVolume);
        PlayerPrefs.SetFloat("SFXVolume", sfxVolume);
        PlayerPrefs.SetInt("MusicEnable", MusicIsEnable ? 1 : 0);
        PlayerPrefs.SetInt("SFXEnable", SFXIsEnable ? 1 : 0);
        PlayerPrefs.Save();
        ApplyVolumeToSources();
    }
    private void ApplyVolumeToSources()
    {
        if (SFXAudioSource != null)
        {
            SFXAudioSource.mute = !SFXIsEnable;
            SFXAudioSource.volume = sfxVolume;
        }
        if (MusicAudioSource != null)
        {
            MusicAudioSource.mute = !MusicIsEnable;
            MusicAudioSource.volume = musicVolume;
        }
    }
    public void PlayTakeDamageSound()
    {
        if (!SFXIsEnable) return;
        SFXAudioSource.PlayOneShot(takeDamageClip);
    }
    public void PlayTakeItemSound()
    {
        if (!SFXIsEnable) return;
        SFXAudioSource.PlayOneShot(takeItemClip);
    }
    public void PlayEatSound()
    {
        if (!SFXIsEnable) return;
        SFXAudioSource.PlayOneShot(eatClip);
    }
    public void PlayMusic()
    {
        if (MusicAudioSource == null) return;
        MusicAudioSource.Play();
    }
}
