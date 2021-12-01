using UnityEngine;

public class LocationBasedAdaptiveMusicLayer : AdaptiveMusicLayer
{
    [Tooltip("May get set in script")]
    public Transform location;

    public AudioLowPassFilter lowPassFilter;

    [HideInInspector] public float locationDistToCamera = Mathf.Infinity;


    public override void Update()
    {
        if (!location) return;

        locationDistToCamera = Vector2.Distance(Camera.main.transform.position, location.position);
        Volume();
        Pan();
        LowPass();
    }

    public override void Volume()
    {
        vol = 1 / (locationDistToCamera / 6);
        vol = Mathf.Clamp(vol, 0, 1);
        vol *= vol;
        vol *= scaledVol;

        if (audioSource.volume != vol)
        {
            if (Mathf.Abs(audioSource.volume - vol) < 0.1f)
            {
                audioSource.volume = vol;
                return;
            }
            float deltaVol = (vol - audioSource.volume) * Time.deltaTime * 4; // << fades to desired volume over 0.25 seconds ish

            audioSource.volume += deltaVol;
        }
        
    }

    void Pan()
    {
        float pan = (location.position.x - Camera.main.transform.position.x) / 5;
        pan = Mathf.Clamp(pan, -1, 1);
        audioSource.panStereo = pan;
    }

    void LowPass()
    {
        float cutoff = (audioSource.volume * 10000) + 10000; // << Cutoff frequecy sweep speed  + minimin cutoff frequency
        lowPassFilter.cutoffFrequency = cutoff;
    }

}
