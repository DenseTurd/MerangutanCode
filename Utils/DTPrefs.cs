using System;
using UnityEngine;

public static class DTPrefs
{
    public static void SetBool(string key, bool val)
    {
        PlayerPrefs.SetInt(key, val ? 1 : 0);
    }

    public static bool GetBool(string Key)
    {
        return PlayerPrefs.GetInt(Key) == 1;
    }


    public static void SetInt(string key, int val)
    {
        PlayerPrefs.SetInt(key, val);
    }

    public static int GetInt(string key)
    {
        return PlayerPrefs.GetInt(key);
    }

    public static void IncrementInt(string Key)
    {
        PlayerPrefs.SetInt(Key, PlayerPrefs.GetInt(Key) + 1);
    }


    public static void SetFloat(string key, float val)
    {
        PlayerPrefs.SetFloat(key, val);
    }

    public static float GetFloat(string key)
    {
        return PlayerPrefs.GetFloat(key);
    }


    public static void SetString(string key, string val)
    {
        PlayerPrefs.SetString(key, val);
    }

    public static string GetString(string key)
    {
        return PlayerPrefs.GetString(key);
    }

    public static void SetColor(string key, Color val) // sort
    {
        string str = val.r + " " + val.g + " " + val.b + " " + val.a;
        PlayerPrefs.SetString(key, str.ToString());
    }

    public static Color GetColor(string key) // sort
    {
        string[] subStrings = (PlayerPrefs.GetString(key).Split(' '));
        float[] floats = new float[4];
        for (int i = 0; i < subStrings.Length; i++)
        {
            floats[i] = float.Parse(subStrings[i]);
        }
        return new Color(floats[0], floats[1], floats[2], floats[3]);
    }

    public static void SetKey(string key, KeyCode val)
    {
        PlayerPrefs.SetString(key, val.ToString());
    }

    public static KeyCode GetKey(string key)
    {
        return StringToKeycode(PlayerPrefs.GetString(key));
    }

    public static KeyCode StringToKeycode(string key)
    {
        return (KeyCode)Enum.Parse(typeof(KeyCode), key);
    }


    public static void SetKeyBinds(string key, KeyBinds binds)
    {
        foreach (var kvp in binds.keyBinds)
        {
            SetKey(key + kvp.Key, kvp.Value);
        }
    }

    public static KeyBinds GetKeyBinds(string key)
    {
        return new KeyBinds
            (
                GetKey(key + Strs.up),
                GetKey(key + Strs.left),
                GetKey(key + Strs.down),
                GetKey(key + Strs.right),
                GetKey(key + Strs.jump),
                GetKey(key + Strs.swing),
                GetKey(key + Strs.dash),
                GetKey(key + Strs.next),
                GetKey(key + Strs.yes),
                GetKey(key + Strs.no)    
            );
    }


    public static bool HasKey(string key)
    {
        return PlayerPrefs.HasKey(key);
    }

    public static void DeleteAll()
    {
        PlayerPrefs.DeleteAll();
    }
}
