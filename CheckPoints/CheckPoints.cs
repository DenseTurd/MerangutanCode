using System.Collections.Generic;
using UnityEngine;

public class CheckPoints : MonoBehaviour
{
    public List<CheckPoint> checkPoints;
    public CheckPoint currentCheckPoint;

    public void AssignCurrentCheckPoint(CheckPoint checkPoint)
    {
        if (currentCheckPoint)
        {
            currentCheckPoint.StopBeingTheCurrentCheckPoint();
        }

        currentCheckPoint = checkPoint;
        currentCheckPoint.BeTheCurrentCheckPoint();
    }
}
