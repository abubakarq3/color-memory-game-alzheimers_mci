using UnityEngine;
using UnityEngine.SceneManagement; // Ensure this is included for scene management

public class MainMenu : MonoBehaviour // 'public' keyword is necessary
{
    public void NewGame()
    {
        SceneManager.LoadSceneAsync(1);
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}