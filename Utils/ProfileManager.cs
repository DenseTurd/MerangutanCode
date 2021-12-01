using System;
using UnityEngine;

public class ProfileManager : MonoBehaviour
{
    public Color inColor = Color.green;
    public Color outColor;
    public void Init()
    {
        //DTPrefs.DeleteAll(); // <<<<<<<<<<<<<<<<<<<<< Uh oh!

        if (!DTPrefs.HasKey(Strs.playerID)) CreatePlayerProfile();
            
        Debug.Log($"Player ID: {DTPrefs.GetString(Strs.playerID)}");

        LoadProfile();

        //TestDTPrefs();
    }

    public void LoadProfile()
    {
        Debug.Log($"Loading profile: {DTPrefs.GetString(Strs.playerID)}");
        LoadKeyBinds();
        // Load all the other things
    }

    public void SaveProfile()
    {
        Debug.Log($"Saving profile: {DTPrefs.GetString(Strs.playerID)}");
        SaveKeyBinds();
        // Save all the other things
    }
    

    public void SaveKeyBinds()
    {
        KeyBinds binds = CurrentInputManagerBinds();
        DTPrefs.SetKeyBinds(DTPrefs.GetString(Strs.playerID), binds);
    }

    public KeyBinds CurrentInputManagerBinds()
    {
        return new KeyBinds
        (
            Overseer.Instance.inputManager.up,
            Overseer.Instance.inputManager.left,
            Overseer.Instance.inputManager.down,
            Overseer.Instance.inputManager.right,

            Overseer.Instance.inputManager.jump,
            Overseer.Instance.inputManager.swing,
            Overseer.Instance.inputManager.dash,

            Overseer.Instance.inputManager.next,
            Overseer.Instance.inputManager.yes,
            Overseer.Instance.inputManager.no
        );
    }

    void LoadKeyBinds()
    {
        KeyBinds binds = DTPrefs.GetKeyBinds(DTPrefs.GetString(Strs.playerID));

        SetInputManagerBinds(binds);
    }

    public void SetInputManagerBinds(KeyBinds binds)
    {
        Overseer.Instance.inputManager.up = binds.keyBinds[Strs.up];
        Overseer.Instance.inputManager.left = binds.keyBinds[Strs.left];
        Overseer.Instance.inputManager.down = binds.keyBinds[Strs.down];
        Overseer.Instance.inputManager.right = binds.keyBinds[Strs.right];

        Overseer.Instance.inputManager.jump = binds.keyBinds[Strs.jump];
        Overseer.Instance.inputManager.swing = binds.keyBinds[Strs.swing];
        Overseer.Instance.inputManager.dash = binds.keyBinds[Strs.dash];

        Overseer.Instance.inputManager.next = binds.keyBinds[Strs.next];
        Overseer.Instance.inputManager.yes = binds.keyBinds[Strs.yes];
        Overseer.Instance.inputManager.no = binds.keyBinds[Strs.no];
    }


    void CreatePlayerProfile()
    {
        DTPrefs.SetString(Strs.playerID, UnityEngine.Random.Range(100000, 999999).ToString());
        DefaultProfile();
    }

    void DefaultProfile()
    {
        KeyBinds profile = defaultKeybinds;
        DTPrefs.SetKeyBinds(DTPrefs.GetString(Strs.playerID), profile);

        DTPrefs.SetInt(DTPrefs.GetString(Strs.playerID) + Strs.DTCoins, 0);

        DTPrefs.SetFloat(DTPrefs.GetString(Strs.playerID) + Strs.masterVolume, 0.75f * 0.75f);
        DTPrefs.SetFloat(DTPrefs.GetString(Strs.playerID) + Strs.musicVolume, 0.75f * 0.75f);
        DTPrefs.SetFloat(DTPrefs.GetString(Strs.playerID) + Strs.sfxVolume, 0.75f * 0.75f);

        // Set ui color default
    }

    public static KeyBinds defaultKeybinds = new KeyBinds
            (
                KeyCode.W,
                KeyCode.A,
                KeyCode.S,
                KeyCode.D,

                KeyCode.Mouse0,
                KeyCode.Space,
                KeyCode.Mouse1,

                KeyCode.E,
                KeyCode.Y,
                KeyCode.N
            );

    void TestDTPrefs()
    {

        Debug.Log("Testing DTPrefs");
        string boolShite = "BoolShite";
        bool booly = true;
        DTPrefs.SetBool(boolShite, booly);
        Debug.Log($"Bool {boolShite} should be true = {DTPrefs.GetBool(boolShite)}");
        booly = false;
        DTPrefs.SetBool(boolShite, booly);
        Debug.Log($"Bool {boolShite} should be false = {DTPrefs.GetBool(boolShite)}");

        string inty = "Inty";
        int i = 66;
        Debug.Log($"Setting int {inty} to {i}");
        DTPrefs.SetInt(inty, i);
        Debug.Log($"Getting int {inty}: {DTPrefs.GetInt(inty)}");
        DTPrefs.IncrementInt(inty);
        Debug.Log($"Incrementing int {inty}: {DTPrefs.GetInt(inty)}");
        DTPrefs.IncrementInt(inty);
        Debug.Log($"Incrementing int {inty}: {DTPrefs.GetInt(inty)}");
        DTPrefs.IncrementInt(inty);
        Debug.Log($"Incrementing int {inty}: {DTPrefs.GetInt(inty)}");

        string floaty = "Floaty";
        float f = 6.9f;
        Debug.Log($"Setting float {floaty} to {f}");
        DTPrefs.SetFloat(floaty, f);
        Debug.Log($"Getting float {floaty}: {DTPrefs.GetFloat("floaty")}");

        string stringy = "Stringy";
        string s = "Boobies";
        Debug.Log($"Setting string {stringy} to {s}");
        DTPrefs.SetString(stringy, s);
        Debug.Log($"Getting string {stringy}: {DTPrefs.GetString(stringy)}");

        string colory = "Colory";
        Debug.Log($"Setting color {colory} to {inColor}");
        DTPrefs.SetColor(colory, inColor);
        Debug.Log($"Getting color {colory} check the out color in the inspector");
        outColor = DTPrefs.GetColor(colory);

        string keyCodey = "Keycodey";
        KeyCode k = KeyCode.Return;
        Debug.Log($"Setting keycode {keyCodey} to {k}");
        DTPrefs.SetKey(keyCodey, k);
        Debug.Log($"Getting keycode {keyCodey}: {DTPrefs.GetKey(keyCodey)}");

        string inputProfiley = "InputProfiley";
        KeyBinds ip = new KeyBinds
            (
                KeyCode.I,
                KeyCode.J,
                KeyCode.K,
                KeyCode.L,

                KeyCode.UpArrow,
                KeyCode.RightAlt,
                KeyCode.DownArrow,

                KeyCode.Return,
                KeyCode.RightArrow,
                KeyCode.LeftArrow
            );
        Debug.Log($"Setting input profile {inputProfiley}");
        foreach (var kvp in ip.keyBinds)
        {
            Debug.Log($"{kvp.Key} is {kvp.Value}");
        }
        DTPrefs.SetKeyBinds(inputProfiley, ip);

        Debug.Log($"Getting input profile {inputProfiley}");
        KeyBinds retrievedProfile = DTPrefs.GetKeyBinds(inputProfiley);
        foreach (var kvp in retrievedProfile.keyBinds)
        {
            Debug.Log($"{kvp.Key} is {kvp.Value}");
        }
    }

    public void LogProfile()
    {
        Debug.Log($"Player ID: {DTPrefs.GetString(Strs.playerID)}");
        Debug.Log($"DTCoins: {DTPrefs.GetInt(DTPrefs.GetString(Strs.playerID) + Strs.DTCoins)}");
        Debug.Log($"Getting key binds");
        KeyBinds keyBinds = DTPrefs.GetKeyBinds(DTPrefs.GetString(Strs.playerID));
        foreach (var kvp in keyBinds.keyBinds)
        {
            Debug.Log($"{kvp.Key} is {kvp.Value}");
        }

        Debug.Log($"Master volume {DTPrefs.GetFloat(DTPrefs.GetString(Strs.playerID) + Strs.masterVolume)}");
        Debug.Log($"Music volume {DTPrefs.GetFloat(DTPrefs.GetString(Strs.playerID) + Strs.musicVolume)}");
        Debug.Log($"Sfx volume {DTPrefs.GetFloat(DTPrefs.GetString(Strs.playerID) + Strs.sfxVolume)}");
    }
}
