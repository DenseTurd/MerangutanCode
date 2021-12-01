using System.Collections.Generic;
using UnityEngine;

public class PlayerAudio : MonoBehaviour
{
    public List<AudioSource> regularJumps;
    public List<AudioSource> wallJumps;
    public List<AudioSource> bounceJumps;
    public List<AudioSource> dashJumps;
    public List<AudioSource> swingJumps;

    public AudioSource plosive;
    public AudioSource swingConnect;
    public AudioSource swingFailToConnect;
    public AudioSource checkpoint;
    public AudioSource land;
    
    [Range(0.5f, 1.5f)] public float jumpPitchMin = 1;
    [Range(0.5f, 1.5f)] public float jumpPitchMax = 1.1f;

    public float coolDownMin;
    public float coolDownMax;
    float nextFireTime = 0;

    void PlaySource(AudioSource source)
    {
        source.volume = AudioManager.sfxVol * AudioManager.masterVol;
        source.volume *= source.volume;
        source.Play();
    }

    void PlaySourceAtPosition(AudioSource source, Vector3 pos)
    {
        source.volume = AudioManager.sfxVol * AudioManager.masterVol;
        source.PlayAtPosition(pos);
    }

    private void Jump(List<AudioSource> audioSources)
    {
        if (Time.time > nextFireTime)
        {
            AudioSource source = audioSources.GetRandom();
            source.pitch = Random.Range(jumpPitchMin, jumpPitchMax);
            PlaySource(source);
            SetNextfireTime();
        }
        PlaySource(plosive);
    }

    void SetNextfireTime() => nextFireTime = Time.time + Random.Range(coolDownMin, coolDownMax);

    #region Jumps
    public void RegularJump() => Jump(regularJumps);
    public void WallJump() => Jump(wallJumps);
    public void BounceJump() => Jump(bounceJumps);
    public void DashJump() => Jump(dashJumps);
    public void SwingJump() => Jump(swingJumps);
    #endregion

    public void SwingConnect(Vector3 pos) => PlaySourceAtPosition(swingConnect, pos);

    public void SwingFailToConnect() => PlaySource(swingFailToConnect);

    public void Checkpoint() => PlaySource(checkpoint);

    public void Land() => PlaySource(land);
}


