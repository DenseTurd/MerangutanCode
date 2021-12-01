using System;
using System.Collections.Generic;
using UnityEngine;

public static class Extensions
{
    public static void PlayAtPosition(this AudioSource source, AudioClip clip, Vector3 position)
    {
        source.panStereo = Pan(position);
        source.volume *= Vol(position);

        // high cut at dist

        source.PlayOneShot(clip);
    }

    public static void PlayAtPosition(this AudioSource source, Vector3 position)
    {
        source.panStereo = Pan(position);
        source.volume *= Vol(position);

        // high cut at dist

        source.Play();
    }

    static float Pan(Vector3 position)
    {
        float pan = (position.x - Camera.main.transform.position.x) / 5;
        return Mathf.Clamp(pan, -1, 1);
    }

    static float Vol(Vector3 position)
    {
        float vol = 1 / ((Vector2.Distance(Camera.main.transform.position, position)) / 3);
        vol = Mathf.Clamp(vol, 0, 1);
        vol *= vol;
        return vol;
    }
    

    public static T GetRandom<T>(this IList<T> list)
    {
        if (list.Count == 0) throw new IndexOutOfRangeException("Cannot get a random element of an empty list");
        return list[UnityEngine.Random.Range(0, list.Count)];
    }  
    
    public static T GetComponentOrComplain<T>(this MonoBehaviour mb)
    {
        T component = mb.GetComponent<T>();
        if (component == null) Debug.LogException(new Exception($"{mb.gameObject.name} cannot find {typeof(T)}"));
        return component;
    }

    public static T FindObjectOfTypeOrComplain<T>(this MonoBehaviour mb) where T : UnityEngine.Object
    {
        T outObj = UnityEngine.Object.FindObjectOfType<T>();
        if (outObj == null) Debug.LogException(new Exception($"{mb.gameObject.name} cannot find {typeof(T)}"));
        return outObj;
    }

    public static Vector3 With(this Vector3 original, float? x = null, float? y = null, float? z = null)
    {
        return new Vector3(x ?? original.x, y ?? original.y, z ?? original.z);
    }

    public static float EaseOut(this float floaty)
    {
        return Mathf.Sin((floaty * Mathf.PI) / 2);// Radians! it's always the radians! that's a nice easing function tho :D
    }
}
