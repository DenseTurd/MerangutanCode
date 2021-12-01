using System;
using UnityEngine;

[RequireComponent(typeof(ProximityActivator))]
public abstract class Enemy : Spawnable, IProximityActivatable, ISectionActivatable
{
    [HideInInspector] public bool active;
    [HideInInspector] public Vector3 spawnPos;

    public virtual void Start() // remember to call base.Start() in any derived class' override Start()'s 
    {
        transform.parent = Overseer.Instance.enemyManager.transform;
        Overseer.Instance.enemyManager.instantiatedEnemies.Add(this);
        spawnPos = transform.position;
    }

    public void Activate()
    {
        active = true;
    }

    public void DeActivate()
    {
        active = false;
    }

    public abstract void ResetMe();
}
