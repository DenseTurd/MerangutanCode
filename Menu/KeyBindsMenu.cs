using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class KeyBindsMenu : MonoBehaviour
{
    static readonly KeyCode[] keyCodes = Enum.GetValues(typeof(KeyCode))
                                                        .Cast<KeyCode>()
                                                        //.Where(k => (int)k < (int)KeyCode.Mouse0) // only if we don't want to detect mouse and joystick inputs
                                                        .ToArray();

    bool awaitingBind;
    string keyToBind;

    public TMP_Text up;
    public TMP_Text left;
    public TMP_Text down;
    public TMP_Text right;

    public TMP_Text jump;
    public TMP_Text swing;
    public TMP_Text dash;

    public TMP_Text next;
    public TMP_Text yes;
    public TMP_Text no;

    public GameObject pressKeyTxt;

    ProfileManager profileManager;

    void OnEnable()
    {
        profileManager = Overseer.Instance.profileManager;
        awaitingBind = false;
        RefreshUI();
    }
    void Bind(string key)
    {
        awaitingBind = true;
        keyToBind = key;
        pressKeyTxt.SetActive(true);
        pressKeyTxt.GetComponent<TMP_Text>().text = "Press key to bind: " + key;
        Debug.Log($"Awaiting input to bind key: {key}");
    }

    void Update()
    {
        if (awaitingBind)
        {
            if (GetCurrentKeyDown() != null)
            {
                KeyBinds binds = profileManager.CurrentInputManagerBinds();
                binds.keyBinds[keyToBind] = (KeyCode)GetCurrentKeyDown();
                profileManager.SetInputManagerBinds(binds);
                profileManager.SaveProfile();

                RefreshUI();

                awaitingBind = false;
                Debug.Log($"Bound {keyToBind} to {binds.keyBinds[keyToBind]}");
            }
        }
    }

    void RefreshUI()
    {
        pressKeyTxt.SetActive(false);

        KeyBinds binds = profileManager.CurrentInputManagerBinds();

        up.text = binds.keyBinds[Strs.up].ToString();
        left.text = binds.keyBinds[Strs.left].ToString();
        down.text = binds.keyBinds[Strs.down].ToString();
        right.text = binds.keyBinds[Strs.right].ToString();

        jump.text = binds.keyBinds[Strs.jump].ToString();
        swing.text = binds.keyBinds[Strs.swing].ToString();
        dash.text = binds.keyBinds[Strs.dash].ToString();

        next.text = binds.keyBinds[Strs.next].ToString();
        yes.text = binds.keyBinds[Strs.yes].ToString();
        no.text = binds.keyBinds[Strs.no].ToString();
    }

    static KeyCode? GetCurrentKeyDown()
    {
        if (!Input.anyKey)
        {
            return null;
        }

        for (int i = 0; i < keyCodes.Length; i++)
        {
            if (Input.GetKey(keyCodes[i]))
            {
                Debug.Log($"Detected key: {keyCodes[i]}");
                return keyCodes[i];
            }
        }

        return null;
    }

    public void Defaults()
    {
        profileManager.SetInputManagerBinds(ProfileManager.defaultKeybinds);
        profileManager.SaveProfile();

        RefreshUI();
    }

    #region buttons
    public void Up()
    {
        Bind(Strs.up);
    }
    public void Left()
    {
        Bind(Strs.left);
    }
    public void Down()
    {
        Bind(Strs.down);
    }
    public void Right()
    {
        Bind(Strs.right);
    }
    public void Jump()
    {
        Bind(Strs.jump);
    }
    public void Swing()
    {
        Bind(Strs.swing);
    }
    public void Dash()
    {
        Bind(Strs.dash);
    }
    public void Next()
    {
        Bind(Strs.next);
    }
    public void Yes()
    {
        Bind(Strs.yes);
    }
    public void No()
    {
        Bind(Strs.no);
    }
    #endregion

}
