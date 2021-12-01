using UnityEngine;
using UnityEngine.UI;

public class AudioMenu : MonoBehaviour
{
    public Slider masterSlider;
    public Slider musicSlider;
    public Slider sfxSlider;
    void Start()
    {
        masterSlider.value = DTPrefs.GetFloat(DTPrefs.GetString(Strs.playerID) + Strs.masterVolume);
        musicSlider.value = DTPrefs.GetFloat(DTPrefs.GetString(Strs.playerID) + Strs.musicVolume);
        sfxSlider.value = DTPrefs.GetFloat(DTPrefs.GetString(Strs.playerID) + Strs.sfxVolume);
    }

    public void MasterVolumeChange(Slider slider)
    {
        Overseer.Instance.audioManager.SetMasterVolume(slider.value);
    }

    public void MusicVolumeChange(Slider slider)
    {
        Overseer.Instance.audioManager.SetMusicVolume(slider.value);
    }

    public void SFXVolumeChange(Slider slider)
    {
        Overseer.Instance.audioManager.SetSFXVolume(slider.value);
    }
}
