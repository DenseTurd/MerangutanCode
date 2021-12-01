using UnityEngine;

public class CollectiblesAudio : MonoBehaviour
{
    public AudioSource spinCoin;
    public AudioSource collectCoin;

    public void SpinCoin()
    {
        PlaySource(spinCoin);
    }

    public void CollectCoin()
    {
        PlaySource(collectCoin);
    }

    void PlaySource(AudioSource source)
    {
        source.volume = AudioManager.sfxVol * AudioManager.masterVol;
        source.volume *= source.volume;
        source.Play();
    }
}
