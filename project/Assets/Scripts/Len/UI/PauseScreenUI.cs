using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseScreenUI : MonoBehaviour
{
    private static bool GameIsPaused;

    public GameObject pauseMenu;
    public GameObject pauseIcon;

    private void Start()
    {
        GameIsPaused = false;
        pauseMenu.SetActive(false);
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

    // Called to continue playing game
    public void Resume()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
        GameIsPaused = false;
    }

    // Called to freeze gameplay and shows menu
    public void Pause()
    {
        pauseMenu.SetActive(true);
        Time.timeScale = 0f;
        GameIsPaused = true;
    }

    // Goes back to the main menu
    public void QuitToMenu()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
        SceneManager.LoadScene("Main Menu", LoadSceneMode.Single);
    }

    // Quits application
    public void Quit()
    {
        Application.Quit();
    }
}