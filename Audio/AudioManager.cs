using System;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public PlayerAudio playerAudio;
    public EnemyAudio enemyAudio;
    public CollectiblesAudio collectiblesAudio;

    MusicAudio musicAudio;
    public AdaptiveMusicManager adaptiveMusicManager;

    public static float masterVol;
    public static float musicVol;
    public static float sfxVol;

    void Awake()
    {
        playerAudio = this.FindObjectOfTypeOrComplain<PlayerAudio>();
        enemyAudio = this.FindObjectOfTypeOrComplain<EnemyAudio>();
        collectiblesAudio = this.FindObjectOfTypeOrComplain<CollectiblesAudio>();
        musicAudio = this.FindObjectOfTypeOrComplain<MusicAudio>();
        adaptiveMusicManager = this.FindObjectOfTypeOrComplain<AdaptiveMusicManager>();
    }

    void Start()
    {
        SetAllLevels();    
    }

    public void InGameMusic()
    {
        musicAudio.InGameMusic();
        adaptiveMusicManager.StartAdaptiveMusicLayers();
    }

    public void SetMasterVolume(float vol) // passes the message to all the things
    {
        DTPrefs.SetFloat(DTPrefs.GetString(Strs.playerID) + Strs.masterVolume, vol);
        masterVol = vol;
        SetAllLevels();
    }

    public void SetMusicVolume(float vol) // passes the message to music things
    {
        DTPrefs.SetFloat(DTPrefs.GetString(Strs.playerID) + Strs.musicVolume, vol);
        musicVol = vol;
        SetMusicLevels();
    }

    public void SetSFXVolume(float vol) // passes the message to sfx things
    {
        DTPrefs.SetFloat(DTPrefs.GetString(Strs.playerID) + Strs.sfxVolume, vol);
        sfxVol = vol;
    }

    void SetAllLevels()
    {
        SetMusicLevels();
    }

    void SetMusicLevels()
    {
        musicAudio.SetLevels();
        adaptiveMusicManager.SetLevels();
    }
}
