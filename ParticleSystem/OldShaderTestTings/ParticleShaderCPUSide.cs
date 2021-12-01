/*  Unfortonately gettin data back from the gpu is too slow to make a gameobject based particle system.
 *  Nice try though :) shaders ain't so hard.
 */

//using System;
//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class ParticleShaderCPUSide : MonoBehaviour
//{
//    public ComputeShader particleShader;
//    public ComputeBuffer buffer;
//    public ParticleData[] particleDataArray;
//    public List<Transform> particles;
//    public int kernel;

//    bool setup;
//    bool debug;

//    DTParticleSystem dTParticleSystem;

//    ParticleData currentSystemSettings;
//    void Awake()
//    {
//        particles = new List<Transform>();
//    }

//    public void SetUp(DTParticleSystem ps)
//    {
//        dTParticleSystem = ps;
//        currentSystemSettings = CreateParticleData();
//        SetParticleDataArray();

//        buffer = new ComputeBuffer(particleDataArray.Length, 100); // Remember the stride will change if the struct changes
//        SetBuffer();

//        setup = true;
//        Debug.Log("SetUp()");
//    }

//    void SetBuffer()
//    {
//        buffer.SetData(particleDataArray);

//        kernel = particleShader.FindKernel("CSMain");
//        particleShader.SetBuffer(kernel, "particleBuffer", buffer);
//    }


//    ParticleData CreateParticleData()
//    {
//        ParticleData particleData = new ParticleData();

//        particleData.ScaleRange = dTParticleSystem.scale;
//        particleData.Speed = dTParticleSystem.speed;
//        particleData.RotationRange = dTParticleSystem.rotation;

//        //public bool RandomDirection;
//        particleData.Direction = dTParticleSystem.direction;
//        particleData.DirectionVariance = dTParticleSystem.directionVariance;

//        //public bool Gravity;
//        particleData.GravityScale = dTParticleSystem.gravityScale;
//        particleData.GravityDirection = dTParticleSystem.gravityDirection;

//        particleData.Drag = dTParticleSystem.drag;

//        particleData.Pos = dTParticleSystem.transform.position + (UnityEngine.Random.insideUnitSphere * dTParticleSystem.area.x);
//        particleData.Rot = Vector3.zero;
//        particleData.Scale = Vector3.one;
//        particleData.lifeTime = Rand.Range(dTParticleSystem.lifeTime.x, dTParticleSystem.lifeTime.y);

//        particleData.Active = 1;

//        return particleData;
//    }

//    void SetParticleDataArray()
//    {
//        particleDataArray = new ParticleData[particles.Count];
//        for (int i = 0; i < particleDataArray.Length; i++)
//        {
//            particleDataArray[i] = currentSystemSettings;
//        }
//    }

//    void Update()
//    {
//        if (setup)
//        {
//            Compute();
//            GetDataBackFromShader();
//            ApplyDataToParticles();
//        }
//    }

//    void Compute()
//    {
//        particleShader.Dispatch(kernel, 1, 1024, 1);
//    } 

//    void GetDataBackFromShader()
//    {
//        buffer.GetData(particleDataArray);
//    }
    
//    void ApplyDataToParticles()
//    {
//        for (int i = 0; i < particles.Count; i++)
//        {
//            SetTransform(particles[i], i);
//        }
//    }

//    public void ResetParticle(Component parti) 
//    {
//        int i = particles.IndexOf(parti.gameObject.transform);
//        particleDataArray[i] = CreateParticleData();
//        SetTransform(parti, i);
//        SetBuffer();
//    }

//    void SetTransform(Component parti, int i)
//    {
//        parti.gameObject.transform.position = particleDataArray[i].Pos;
//        parti.gameObject.transform.rotation = Quaternion.Euler(particleDataArray[i].Rot);
//        parti.gameObject.transform.localScale = particleDataArray[i].Scale;
//    }

//    void OnDestroy()
//    {
//        buffer.Release();
//        Debug.Log("Particle buffer released.. In theory");
//    }
//}

//public struct ParticleData
//{
//    public Vector2 ScaleRange;
//    public Vector2 Speed;
//    public float RotationRange;

//    //public bool RandomDirection;
//    public Vector3 Direction;
//    public float DirectionVariance;

//    //public bool Gravity;
//    public float GravityScale;
//    public Vector3 GravityDirection;

//    public float Drag;

//    public Vector3 Pos;
//    public Vector3 Rot;
//    public Vector3 Scale;
//    public float lifeTime;

//    public int Active;
//}
