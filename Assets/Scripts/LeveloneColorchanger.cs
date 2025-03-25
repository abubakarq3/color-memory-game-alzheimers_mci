using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class LeveloneColorchanger : MonoBehaviour
{
    public GameObject cube;                // Referencia al cubo
    public Material[] colorMaterials;      // Materiales disponibles
    public TextMeshProUGUI scoreText;      // Texto para el puntaje

    private Material correctCubeColor;    // Color objetivo para el cubo
    private int score = 0;                // Puntaje del jugador
    private int round = 0;                // N�mero de rondas
    private bool isRoundInProgress = false;
 public GameOverScreen GameOverScreen;
    void OnEnable()
    {
        Brush.OnBrushColored += HandleBrushColored;
        Brush.OnObjectPainted += HandleObjectPainted;
    }

    void OnDisable()
    {
        Brush.OnBrushColored -= HandleBrushColored;
        Brush.OnObjectPainted -= HandleObjectPainted;
    }

    void Start()
    {
        UpdateScoreText();
        StartNewRound();
    }

    void StartNewRound()
    {
        if (colorMaterials == null || colorMaterials.Length == 0)
        {
            Debug.LogError("El array colorMaterials est� vac�o. Asigna materiales en el Inspector.");
            return;
        }

        isRoundInProgress = true; // Indica que la ronda est� activa
        correctCubeColor = colorMaterials[Random.Range(0, colorMaterials.Length)];
        cube.GetComponent<Renderer>().material = correctCubeColor;

        Debug.Log($"Nueva ronda. Color objetivo: {correctCubeColor.color}");

        StartCoroutine(ShowCorrectColor());
    }

    System.Collections.IEnumerator ShowCorrectColor()
    {
        yield return new WaitForSeconds(1);
        cube.GetComponent<Renderer>().material.color = Color.white; // Resetea el color del cubo
    }

    void HandleBrushColored(Color brushColor)
    {
        Debug.Log($"El Brush cambi� de color a: {brushColor}");
    }

    void HandleObjectPainted(Color objectColor)
    {
        if (!isRoundInProgress) return; // Evita procesar eventos si la ronda ya termin�

        Debug.Log($"Objeto pintado con color: {objectColor}");

        if (objectColor == correctCubeColor.color)
        {
            Debug.Log($"�CORRECT COLOR!  {score}");

             score++;
            round++;
        }
        else
        {
            GameOver();
            round = 3;
            Debug.Log($"INCORRECT COLOR.   {score}");
        }

        UpdateScoreText();
        isRoundInProgress = false; // Marca el fin de la ronda

        if (score >= 3)
        {
              // Display the score and show the final score
        Debug.Log("Game Over! Your score: " + score + "/3");

        // Update the score text to show the final result
        scoreText.text = "Game Over! Final Score: " + score + "/3";
            StartCoroutine(WaitAndLoadNextScene());
        }
        else
        {
            StartCoroutine(WaitBeforeNextRound());
        }
    }

    System.Collections.IEnumerator WaitBeforeNextRound()
    {
        yield return new WaitForSeconds(2); // Pausa para mostrar el resultado de la ronda
        StartNewRound();
    }

    System.Collections.IEnumerator WaitAndLoadNextScene()
    {
        yield return new WaitForSeconds(3); // Pausa antes de cargar la siguiente escena
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    void UpdateScoreText()
    {
        if (scoreText != null)
        {
            scoreText.text = $"Score: {score}";
        }
    }
      public void GameOver()
    {
        GameOverScreen.Setup(score);
    }
}