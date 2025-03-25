using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class LeveltwoColorchanger : MonoBehaviour
{
    public GameObject cylinder;                // Referencia al cilindro
    public GameObject cube;                    // Referencia al cubo
    public Material[] colorMaterials;          // Materiales disponibles
    public TextMeshProUGUI scoreText;          // Texto para el puntaje

    private Material correctCylinderColor;     // Color objetivo para el cilindro
    private Material correctCubeColor;         // Color objetivo para el cubo

    private int score = 0;                     // Puntaje del jugador
    private int round = 0;                     // Número de rondas

    private bool allObjectsPainted = false;    // Indica si todos los objetos han sido pintados al menos una vez

    public GameOverScreen GameOverScreen;

    void Start()
    {
        UpdateScoreText();
        StartCoroutine(ShowRandomColorsForTime());
    }

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

    IEnumerator ShowRandomColorsForTime()
    {
        // Asignar colores aleatorios al cilindro y al cubo
        correctCylinderColor = colorMaterials[Random.Range(0, colorMaterials.Length)];
        correctCubeColor = colorMaterials[Random.Range(0, colorMaterials.Length)];

        cylinder.GetComponent<Renderer>().material = correctCylinderColor;
        cube.GetComponent<Renderer>().material = correctCubeColor;

        Debug.Log($"Cilindro color objetivo: {correctCylinderColor.color}, Cubo color objetivo: {correctCubeColor.color}");

        // Mostrar colores por 3 segundos
        yield return new WaitForSeconds(3);

        // Revertir a blanco
        cylinder.GetComponent<Renderer>().material.color = Color.white;
        cube.GetComponent<Renderer>().material.color = Color.white;

        allObjectsPainted = false; // Reinicia el estado para la nueva ronda
    }

    void HandleBrushColored(Color brushColor)
    {
        Debug.Log($"Brush tomó el color: {brushColor}");
    }

    void HandleObjectPainted(Color objectColor)
    {
        Debug.Log($"Objeto pintado con color: {objectColor}");

        // Verificar si todos los objetos han sido pintados al menos una vez
        if (cylinder.GetComponent<Renderer>().material.color != Color.white &&
            cube.GetComponent<Renderer>().material.color != Color.white)
        {
            allObjectsPainted = true; // Marca que todos los objetos han sido pintados
            StartCoroutine(EndRound());
        }
    }

    IEnumerator EndRound()
    {
        if (!allObjectsPainted) yield break;

        yield return new WaitForSeconds(1);

        // Verificar si ambos objetos tienen el color correcto
        bool isCylinderCorrect = cylinder.GetComponent<Renderer>().material.color == correctCylinderColor.color;
        bool isCubeCorrect = cube.GetComponent<Renderer>().material.color == correctCubeColor.color;

        if (isCylinderCorrect && isCubeCorrect)
        {
            score++; // Solo suma puntos si ambos son correctos
            round++;
            Debug.Log("Ambos colores son correctos. Puntaje incrementado.");
        }
        else
        {
            GameOver();
            round = 3;
            Debug.Log("Los colores no son correctos. No se suma puntaje.");
        }

        UpdateScoreText();

        round++;

        EndGame();
        ResetRound();
        
    }

    void ResetRound()
    {
        cylinder.GetComponent<Renderer>().material.color = Color.white;
        cube.GetComponent<Renderer>().material.color = Color.white;

        StartCoroutine(ShowRandomColorsForTime());
    }

    void EndGame()
    {
        if (score >= 3)
        {
            Debug.Log("Juego terminado. Puntaje suficiente para avanzar.");
            if (scoreText != null)
            {
                scoreText.text = $"Game Over! Final Score: {score}";
            }
            StartCoroutine(WaitAndLoadNextScene());
        }
    }

    IEnumerator WaitAndLoadNextScene()
    {
        yield return new WaitForSeconds(5);
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