using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicAudio : MonoBehaviour
{
    [Header ("Audio Sources")]
    public AudioSource drum;
    public AudioSource bass;
    public AudioSource sax;
    

    [Header ("Audio Clips")]
    [SerializeField] public AudioClip gameStart;
    [SerializeField] public AudioClip mainMenu;
    [SerializeField] public AudioClip pauseMenu;
    [SerializeField] public AudioClip inGameMusic;

    [Header("Levels")]
    //[SerializeField] [Range(0, 1)] float musicMasterLevel; music volume is retrieved from playerPrefs.
    [SerializeField] [Range(0, 1)] float startLevel;
    [SerializeField] [Range(0, 1)] float mainMenuLevel;
    [SerializeField] [Range(0, 1)] float pauseMenuLevel;
    [SerializeField] [Range(0, 1)] float inGameLevel;

    public void InGameMusic()
    {
        drum.Play();
        bass.Play();
        sax.Play();
    }    

    public void SetLevels()
    {
        SetLevelsBasedOnPlayState(inGameLevel);
    }

    void SetLevelsBasedOnPlayState(float playState) // call this method when you want to change the volume if the game is paused etc
    {
        
        drum.volume = playState * AudioManager.musicVol * AudioManager.masterVol;
        bass.volume = playState * AudioManager.musicVol * AudioManager.masterVol;
        sax.volume = playState * AudioManager.musicVol * AudioManager.masterVol;
    }
}
