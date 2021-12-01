using System.Collections.Generic;
using UnityEngine;

public class KeyBinds 
{
    public Dictionary<string, KeyCode> keyBinds;
    public KeyBinds
    (
        KeyCode up,
        KeyCode left,
        KeyCode down,
        KeyCode right,

        KeyCode jump,
        KeyCode swing,
        KeyCode dash,

        KeyCode next, 
        KeyCode yes,
        KeyCode no
    )
    {
        keyBinds = new Dictionary<string, KeyCode>();
        keyBinds.Add(Strs.up, up);
        keyBinds.Add(Strs.left, left);
        keyBinds.Add(Strs.down, down);
        keyBinds.Add(Strs.right, right);

        keyBinds.Add(Strs.jump, jump);
        keyBinds.Add(Strs.swing, swing);
        keyBinds.Add(Strs.dash, dash);

        keyBinds.Add(Strs.next, next);
        keyBinds.Add(Strs.yes, yes);
        keyBinds.Add(Strs.no, no);
    }
}
