using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameSession : MonoBehaviour
{

    [SerializeField] int playerLives = 03;
    [SerializeField] int score = 0;

    [SerializeField] TextMeshProUGUI livesText;
    [SerializeField] TextMeshProUGUI scoreText;

    void Awake()
    {
        int numGameSession = FindObjectsOfType<GameSession>().Length;

        if(numGameSession > 1)
        {
            Destroy(gameObject);
        }

        else
        {
            DontDestroyOnLoad(gameObject);
        }
    }

    void Start()
    {
        livesText.text = playerLives.ToString();
        scoreText.text = score.ToString();
    }

    public void ProcessPlayerDeath()
    {
        if(playerLives > 1)
        {
            TakeLive();
        }

        else
        {
            ResetGameSession();
        }
    }

    public void AddScore(int pointScore)
    {
        score += pointScore;
        scoreText.text = score.ToString();
    }

    void TakeLive()
    {
        playerLives--;

        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);
        livesText.text = playerLives.ToString();
    }

    void ResetGameSession()
    {
        SceneManager.LoadScene(0);
        Destroy(gameObject);
    }
}
