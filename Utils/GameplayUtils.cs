using UnityEngine;

public class GameplayUtils : MonoBehaviour
{
    public KeyCode restart = KeyCode.Backspace;
    public KeyCode help = KeyCode.H;
    void Update()
    {
        if (Input.GetKeyDown(restart)) Overseer.Instance.fails.RestartLevel();
        if (Input.GetKeyDown(help)) Debug.Log(helpstr);
    }

    string helpstr = "\nWASD to move" +
        "\nSpace to swing" +
        "\nLeft click to jump" +
        "\nRight click to dash" +
        "\nIf you jump as you hit the ground you will do a bounce jump" +
        "\nYou can dash in the air, doing so unlocks an air jump" +
        "\nYou can wall jump by holding the direction to a wall whilst sliding against it and jumping" +
        "\nYou can influence your swing with WASD" +
        "\nIf you're holding W when you release your swing you will jump off the rope, a swing jump" +
        "\nE advances conversations" +
        "\nY/N to make dialogue choices" +
        "\nBackspace restarts the current scene" +
        "\nTab closes and opens the in game debugger" +
        "\nYou can rebind all the keys in the pause menu (Esc)";
}
