using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    public GameObject mainPanel;

    [SerializeField] public List<GameObject> panels;
    float playSpeed = 1;

    public void Pause()
    {
        if (Time.timeScale > 0)
        {
            playSpeed = Time.timeScale;
            Time.timeScale = 0;
            OpenPanel(mainPanel);
        }
        else
        {
            ClosePanels();
            Time.timeScale = playSpeed;
        }

        
    }

    public void OpenPanel(GameObject panel)
    {
        ClosePanels();
        panel.SetActive(true);
    }
    
    public void ClosePanels()
    {
        foreach (GameObject p in panels)
        {
            p.SetActive(false);
        }
    }

    public void Back()
    {
        OpenPanel(mainPanel);
    }
}
