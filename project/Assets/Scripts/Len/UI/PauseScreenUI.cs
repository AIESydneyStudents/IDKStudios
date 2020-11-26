using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseScreenUI : MonoBehaviour
{
    public GameObject pauseMenu;

    private void Start()
    {
        pauseMenu.SetActive(false);
    }

    public void Pause()
    {
        GameEventManager.Instance.TogglePauseGame();
    }

    // Called to continue playing game
    public void Resume()
    {
        Pause();
    }

    // Goes back to the main menu
    public void QuitToMenu()
    {
        pauseMenu.SetActive(false);
        SceneManager.LoadScene("Main Menu", LoadSceneMode.Single);
    }

    // Quits application
    public void Quit()
    {
        Application.Quit();
    }
}