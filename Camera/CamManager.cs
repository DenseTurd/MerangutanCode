using UnityEngine;
using Cinemachine;

public class CamManager : MonoBehaviour
{
    public CinemachineVirtualCamera vCam;
    public CamOffsetter camOffsetter;
    public void Init()
    {
        vCam = this.FindObjectOfTypeOrComplain<CinemachineVirtualCamera>();
        vCam.Follow = Overseer.Instance.player.transform;
        vCam.LookAt = Overseer.Instance.player.GetComponentInChildren<CamLookTarget>().transform;
        camOffsetter = this.GetComponentOrComplain<CamOffsetter>();
    }
}
