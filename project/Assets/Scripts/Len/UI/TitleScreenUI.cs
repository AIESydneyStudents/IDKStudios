using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleScreenUI : MonoBehaviour
{
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private void Start()
    {
        Time.timeScale = 1.0f;
    }

    public void ContinueGame()
    {
        // need to add this
    }

    public void StartNewGame()
    {
        SceneManager.LoadScene("Tea shop", LoadSceneMode.Single);
    }

    public void Exit()
    {
        Application.Quit();
    }
}
