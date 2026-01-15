using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameOverManager : MonoBehaviour
{
    public GameObject gameOverPanel;  // Panel de Game Over
    public Button restartButton;    // Botón de reiniciar
    public TextMeshProUGUI scoreText;  // Texto del score (usa Text si no tienes TMP)

    public GameObject snake;       // GameObject de Snake
    public GameObject food;   // GameObject de Food

    private Snake snakeScript;
    private int currentScore = 0;

    private void Start()
    {
        // Ocultar el panel al inicio
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(false);
        }

        // Busca el script de Snake
        snakeScript = FindObjectOfType<Snake>();

        // Funcion boton
        if (restartButton != null)
        {
            restartButton.onClick.AddListener(RestartGame);
        }

        // Inicializa score
        UpdateScore(0);
    }

    public void UpdateScore(int score)
    {
        currentScore = score;
    }

    public void ShowGameOver()
    {
        // Actualiza el texto del score
        if (scoreText != null)
        {
            scoreText.text = "Score: " + currentScore.ToString();
        }

        // Oculta snake y todos sus segmentos!!
        if (snakeScript != null)
        {
            snakeScript.HideSegments();
        }
        if (snake != null)
        {
            snake.SetActive(false);
        }
        if (food != null)
        {
            food.SetActive(false);
        }

        // Mostrar panel
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);
        }

        // Pausar el juego
        Time.timeScale = 0f;
    }

    public void RestartGame()
    {
        // Ocultar panel
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(false);
        }

        // Mostrar Snake y Food
        if (snake != null)
        {
            snake.SetActive(true);
        }
        if (food != null)
        {
            food.SetActive(true);
        }

        Time.timeScale = 1f;  // Reanudar el tiempo

        UpdateScore(0); // Resetear score

        // Llama al reset del Snake (destruye los segmentos viejos)
        if (snakeScript != null)
        {
            snakeScript.ResetState();
        }

        // Mostrar los segmentos (solo la cabeza después del reset)
        if (snakeScript != null)
        {
            snakeScript.ShowSegments();
        }
    }
}