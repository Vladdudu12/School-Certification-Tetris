using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class GameManager : MonoBehaviour
{
    public bool _isPause = false;

    public bool _isGameOver = false;
    [SerializeField]
    private int _score = 0 ;
    [SerializeField]
    public int _level = 1;
    [SerializeField]
    public int[] _levelReq;

    private int highscore;

    private UIManager UIManager;
    private Transform PieceManager;
    public void Start()
    {
        highscore = PlayerPrefs.GetInt("highscore");

        PieceManager = GameObject.Find("Pieces").GetComponent<Transform>();
        if (PieceManager == null)
        {
            Debug.LogError("The PieceManager is NULL");
        }

        UIManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        if (UIManager == null)
        {
            Debug.LogError("The UIManager is NULL");
        }
    }

    private void Update()
    {

        if (PieceManager.childCount >= 150)
        {
            _isGameOver = true;
        }

        if (_isGameOver == true)
        {
            CallGameOverPopup();
            if (Input.GetKeyDown(KeyCode.R)) 
            {
                SceneManager.LoadScene(0);
            }
        }
    }

    public void GameOver()
    {
        _isGameOver = true;

    }

    private void CallGameOverPopup()
    {
        if (_score > highscore)
        {
            highscore = _score;

            PlayerPrefs.SetInt("highscore", highscore);
            PlayerPrefs.Save();
        }
        UIManager.GameOverMenu(_score);

    }

    public void Pause()
    {
        _isPause = !_isPause;
    }

    public void AddScore(int value)
    {
        _score += value;
        Debug.Log(_score);
        UIManager.UpdateScore(_score);

        //to add back to back tetris scores
    }

    public void RaiseLevel()
    {
        _level++;
    }

    public int GetLevelReq(int level)
    {
        return _levelReq[level];
    }
}
