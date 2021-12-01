using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CamOffsetter : MonoBehaviour
{
    public List<Vector3> desiredOffsets;
    [HideInInspector] public Vector3 originalOffset = Vector3.zero;
    Vector3 desiredOffset;
    CinemachineCameraOffset cmOffset;

    Vector3 combinedOffsets;
    Vector3 adjust;

    bool initialised;

    public void Init()
    {
        desiredOffsets = new List<Vector3>();
        cmOffset = Overseer.Instance.camManager.vCam.GetComponentOrComplain<CinemachineCameraOffset>();
        initialised = true;
    }

    void Update()
    {
        if (!initialised) return;
        if (desiredOffsets.Count != 0)
        {
            combinedOffsets = Vector3.zero;
            for (int i = 0; i < desiredOffsets.Count; i++)
            {
                combinedOffsets += desiredOffsets[i];
            }
            desiredOffset = combinedOffsets / desiredOffsets.Count;

            if (Vector3.Distance(cmOffset.m_Offset, desiredOffset) > Mathf.Epsilon)
            {
                adjust = (desiredOffset - cmOffset.m_Offset) * Time.deltaTime * 2;
                cmOffset.m_Offset += adjust;
            }

            desiredOffsets.Clear();
        }
        else
        {
            if (Vector2.Distance(cmOffset.m_Offset, originalOffset) > Mathf.Epsilon)
            {
                adjust = (originalOffset - cmOffset.m_Offset) * Time.deltaTime * 2;
                cmOffset.m_Offset += adjust;
            }
            else
            {
                cmOffset.m_Offset = originalOffset;
            }
        }
    }
}
