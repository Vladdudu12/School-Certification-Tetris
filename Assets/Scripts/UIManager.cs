using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private Text _scoreText;
    [SerializeField]
    private Text _levelText;

    [SerializeField]
    private TextMeshProUGUI _highScoreText;
    [SerializeField]
    private TextMeshProUGUI _currentScoreText;

    [SerializeField]
    private GameObject _pauseMenu;
    [SerializeField]
    private GameObject _gameOverMenu;
    // Start is called before the first frame update
    void Start()
    {
        _scoreText.text = "Score: 0";
        _levelText.text = "Level: 1";
    }

    private void Update()
    {
        if (FindObjectOfType<GameManager>()._isGameOver == true)
        {
        }
    }

    public void GameOverMenu(int currentScore)
    {
        _gameOverMenu.SetActive(true);
        _highScoreText.text = "HIGH SCORE: <size=+15><color=white>" + PlayerPrefs.GetInt("highscore").ToString() + "</color>";
        _currentScoreText.text = "CURRENT SCORE: <size=+15><color=white>" + currentScore.ToString() + "</color>";
    }

    public void UpdateScore(int score)
    {
        _scoreText.text = "Score: " + score.ToString();
    }
    public void UpdateLevel(int level)
    {
        _levelText.text = "Level: " + level.ToString();
    }

    public void PauseButton()
    {
        _pauseMenu.SetActive(true);
        FindObjectOfType<GameManager>().Pause();
    }

    public void UnpauseButton()
    {
        _pauseMenu.SetActive(false);
        FindObjectOfType<GameManager>().Pause();
    }

    public void RetryButton()
    {
        _pauseMenu.SetActive(false);
        //loading screen
        SceneManager.LoadScene(1);
    }


    public void ExitButton()
    {
        _pauseMenu.SetActive(false);
        //loading screen
        SceneManager.LoadScene(0);
    }
}

