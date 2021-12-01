using UnityEngine.SceneManagement;

public class LevelEnd : Spawnable
{
    public void EndLevel()
    {
        SceneManager.LoadScene("Hub");
    }
}
