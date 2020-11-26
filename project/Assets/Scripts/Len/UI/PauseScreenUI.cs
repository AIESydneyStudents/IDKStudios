using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseScreenUI : MonoBehaviour
{
    public GameObject pauseMenu;

    public void Pause()
    {
        GameEventManager.Instance.TogglePauseGame();
        InputController.Instance.DisableInteraction();
    }

    // Called to continue playing game
    public void Resume()
    {
        Pause();
        InputController.Instance.EnableInteraction();
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