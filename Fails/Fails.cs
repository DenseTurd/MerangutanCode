using UnityEngine;
using UnityEngine.SceneManagement;

public class Fails : MonoBehaviour
{
    Player player;
    void Start()
    {
        player = Overseer.Instance.player;
    }

    void Update()
    {
        FallCheck();
    }

    void FallCheck()
    {
        if (CurrentSectionAssigner.currentSection)
        {
            if (player.transform.position.y < CurrentSectionAssigner.currentSection.transform.position.y - 50)
            {
                Respawn();
            }
        }
    }

    public void Respawn()
    {
        if (Overseer.Instance.checkPoints.currentCheckPoint)
        {
            player.transform.position = Overseer.Instance.checkPoints.currentCheckPoint.transform.position;
            player.health.hp = 0;
            Overseer.Instance.PlayerRespawn();
        }
        else
        {
            RestartLevel();
        }
    }

    public void RestartLevel()
    {
        Debug.Log("Reload scene");
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
