using UnityEngine;

public class AdaptiveMusicLayer : MonoBehaviour
{
    public AudioSource audioSource;

    [HideInInspector]public float vol;

    public float scaledVol;

    public virtual void Update()
    {
        Volume();
    }

    public virtual void Volume()
    {
        float realVol = vol * scaledVol;

        if (audioSource.volume != realVol)
        {
            if (Mathf.Abs(audioSource.volume - realVol) < 0.1f)
            {
                audioSource.volume = realVol;
                return;
            }
            float deltaVol = (realVol - audioSource.volume) * Time.deltaTime * 4; // << fades to desired volume over 0.25 seconds ish

            audioSource.volume += deltaVol;
        }  
    }

    public void SetLevels()
    {
        scaledVol = AudioManager.musicVol * AudioManager.masterVol;
    }
}
