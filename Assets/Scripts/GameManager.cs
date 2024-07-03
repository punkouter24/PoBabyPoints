using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private float gameDuration = 8f; // Change game duration to 4 seconds
    private float gameTimer = 0f;
    private bool gameEnded = false;
    private int circlesTouched = 0;
    private int totalCircles = 3; // Number of circles in the scene
    private int score = 0;
    private int level = 1;
    private bool gameStarted = false;
    private bool isBabyMode = false; // Add this line

    public Text scoreText;
    public Text highScoresText;
    public Text levelText; // Add a Text element to display the level
    public Text gameOverText; // Add a Text element to display game over
    public Button playButton; // Use a single button for both play and restart
    public Button quitButton;
    public Button babyButton; // Add this line

    private List<CircleMovement> circles;
    private HighScoreManager highScoreManager;

    void Start()
    {
        // Initialize UI elements
        scoreText.text = "Score: 0";
        levelText.text = "Level: 1"; // Initialize level text
        gameOverText.gameObject.SetActive(false); // Hide game over text initially
        playButton.onClick.AddListener(StartOrRestartGame); // Add listener for play button
        quitButton.onClick.AddListener(QuitGame);
        babyButton.onClick.AddListener(StartBabyMode); // Add this line

        highScoreManager = FindObjectOfType<HighScoreManager>();
        if (highScoreManager == null)
        {
            Debug.LogError("HighScoreManager not found in the scene!");
        }

        ShowMainMenu();

        // Initialize the circles list
        circles = new List<CircleMovement>();

        LoadAndDisplayHighScores();
    }

    void Update()
    {
        if (gameStarted && !gameEnded)
        {
            if (!isBabyMode) // Add this condition
            {
                gameTimer += Time.deltaTime;
                if (gameTimer >= gameDuration)
                {
                    EndGame();
                }
            }
        }
    }

    public void CircleTouched(CircleMovement circle)
    {
        circlesTouched++;
        score++;
        scoreText.text = "Score: " + score;

        // Remove the touched circle from the list
        circles.Remove(circle);

        // Destroy the circle gameObject after interaction
        Destroy(circle.gameObject);

        if (circlesTouched >= totalCircles)
        {
            if (!isBabyMode) // Add this condition
            {
                NextLevel();
            }
            else
            {
                // In baby mode, just keep spawning new circles
                var newCircle = InstantiateCircle();
                if (newCircle != null)
                {
                    circles.Add(newCircle);
                    newCircle.ResetPositionAndSpeed(level);
                }
                else
                {
                    Debug.LogError("Failed to instantiate new circle. Check if the prefab is correctly set in the Resources folder.");
                }
            }
        }
    }

    private void NextLevel()
    {
        level++;
        circlesTouched = 0;
        levelText.text = "Level: " + level; // Update level text

        // Clear existing circles
        foreach (var circle in circles)
        {
            Destroy(circle.gameObject);
        }
        circles.Clear();

        // Spawn new circles for the next level
        for (int i = 0; i < totalCircles; i++)
        {
            var newCircle = InstantiateCircle();
            if (newCircle != null)
            {
                circles.Add(newCircle);
                newCircle.ResetPositionAndSpeed(level);
            }
            else
            {
                Debug.LogError("Failed to instantiate new circle. Check if the prefab is correctly set in the Resources folder.");
            }
        }
    }

    private void EndGame()
    {
        gameEnded = true;
        gameStarted = false;
        Debug.Log($"Game Over! Final Score: {score}, Time: {gameTimer} seconds.");

        // Save high score
        highScoreManager.SaveHighScore(score);

        // Show game over screen
        gameOverText.gameObject.SetActive(true);
        playButton.gameObject.SetActive(true);
        playButton.GetComponentInChildren<Text>().text = "PLAY"; // Change text to "PLAY"
        quitButton.gameObject.SetActive(true);
        highScoresText.gameObject.SetActive(true); // Show high scores text
        scoreText.enabled = false;
        levelText.enabled = false;
    }

    private void ShowMainMenu()
    {
        scoreText.enabled = false;
        levelText.enabled = false; // Hide level text in the main menu
        highScoresText.gameObject.SetActive(true); // Show high scores text
        playButton.gameObject.SetActive(true);
        quitButton.gameObject.SetActive(true);
        playButton.GetComponentInChildren<Text>().text = "PLAY"; // Change text to "PLAY"

        LoadAndDisplayHighScores();
    }

    private void HideMainMenu()
    {
        scoreText.enabled = true;
        levelText.enabled = true; // Show level text when the game starts
        highScoresText.gameObject.SetActive(false); // Hide high scores text during gameplay
        playButton.gameObject.SetActive(false);
        quitButton.gameObject.SetActive(false); // Hide quit button during gameplay
    }

    private void StartOrRestartGame()
    {
        isBabyMode = false; // Reset baby mode
        score = 0;
        circlesTouched = 0;
        level = 1;
        gameTimer = 0f;
        gameEnded = false;
        gameStarted = true;
        scoreText.text = "Score: 0";
        levelText.text = "Level: 1"; // Reset level text
        HideMainMenu();
        gameOverText.gameObject.SetActive(false); // Hide game over text
        babyButton.gameObject.SetActive(false); // Hide baby mode button

        // Clear existing circles
        foreach (var circle in circles)
        {
            Destroy(circle.gameObject);
        }
        circles.Clear();

        // Spawn initial circles
        for (int i = 0; i < totalCircles; i++)
        {
            var newCircle = InstantiateCircle();
            if (newCircle != null)
            {
                circles.Add(newCircle);
                newCircle.ResetPositionAndSpeed(level);
            }
            else
            {
                Debug.LogError("Failed to instantiate new circle. Check if the prefab is correctly set in the Resources folder.");
            }
        }
    }

    private void StartBabyMode()
    {
        isBabyMode = true;
        score = 0;
        circlesTouched = 0;
        level = 1;
        gameTimer = 0f;
        gameEnded = false;
        gameStarted = true;
        scoreText.text = "Score: 0";
        levelText.text = "Level: 1";
        HideMainMenu();
        gameOverText.gameObject.SetActive(false); // Hide game over text
        babyButton.gameObject.SetActive(false); // Hide baby mode button

        // Clear existing circles
        foreach (var circle in circles)
        {
            Destroy(circle.gameObject);
        }
        circles.Clear();

        // Spawn initial circles
        for (int i = 0; i < totalCircles; i++)
        {
            var newCircle = InstantiateCircle();
            if (newCircle != null)
            {
                circles.Add(newCircle);
                newCircle.ResetPositionAndSpeed(level);
            }
            else
            {
                Debug.LogError("Failed to instantiate new circle. Check if the prefab is correctly set in the Resources folder.");
            }
        }

        // Hide Score and Level text for Baby Mode
        scoreText.enabled = false;
        levelText.enabled = false;
    }

    private void QuitGame()
    {
        Application.Quit();
    }

    public bool IsGameStarted()
    {
        return gameStarted;
    }

    private CircleMovement InstantiateCircle()
    {
        string[] prefabNames = { "CirclePrefab1", "CirclePrefab2", "CirclePrefab3" };
        string randomPrefabName = prefabNames[Random.Range(0, prefabNames.Length)];
        
        GameObject circlePrefab = Resources.Load<GameObject>(randomPrefabName);
        if (circlePrefab != null)
        {
            GameObject newCircle = Instantiate(circlePrefab);
            return newCircle.GetComponent<CircleMovement>();
        }
        else
        {
            Debug.LogError($"{randomPrefabName} not found in Resources folder.");
            return null;
        }
    }

    private void LoadAndDisplayHighScores()
    {
        highScoreManager.LoadHighScores();
        List<int> highScores = highScoreManager.GetHighScores();
        highScoresText.text = "High Scores:\n";
        for (int i = 0; i < highScores.Count; i++)
        {
            highScoresText.text += (i + 1) + ". " + highScores[i] + "\n";
        }
    }
}
