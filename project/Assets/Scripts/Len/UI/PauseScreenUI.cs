using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseScreenUI : MonoBehaviour
{
    private static bool GameIsPaused;

    public GameObject pauseMenu;
    public GameObject pauseIcon;

    private void Start()
    {
        GameIsPaused = false;    
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (GameIsPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    public void Pause()
    {

    }

    public void Resume()
    {

    }

    public void QuitToMenu()
    {

    }

}
