using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleScreenUI : MonoBehaviour
{
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]

    static void OnStartUp()
    {
        SceneManager.LoadScene("Main Menu", LoadSceneMode.Single);
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
