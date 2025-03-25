

using System.Collections;
using UnityEngine;
using UnityEngine.UI; // Add this for Unity's UI elements
using UnityEngine.SceneManagement;

public class GameOverScreen : MonoBehaviour
{
    public Text ScoreText; // For Unity's default UI Text (change to TextMeshProUGUI if using TextMeshPro)

    public void Setup(int score)
    {
        gameObject.SetActive(true);
        ScoreText.text = score.ToString() + " POINTS"; // Fix incorrect usage of 'ScoreText'
    }


    public void RestartButton()
    {
       // SceneManager.LoadScene("Level 1");
        SceneManager.LoadSceneAsync(1);
    }

    public void ExitButton()
    {
        SceneManager.LoadScene("GameMaster");
    }

}
