using UnityEngine;

[System.Serializable]
public class Dialogue 
{
    [HideInInspector]
    public string name = "NoName";

    [TextArea(3, 10)]
    public string[] sentences;
}
