using System;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;
    private Text _scoreText;
    public int score = 10;
    public int lastScore;

    private void Awake()
    {
        _scoreText = GameObject.Find("ScoreText").GetComponent<Text>();
        MakeSingleton();
    }

    private void Start()
    {
        AddScore(0);
    }

    private void Update()
    {
        if (_scoreText == null)
        {
            _scoreText = GameObject.Find("ScoreText").GetComponent<Text>();
            _scoreText.text = score.ToString();
        }
    }

    private void MakeSingleton()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }
    
    public void AddScore(int value)
    {
        score += value;

        if (score > PlayerPrefs.GetInt("HighScore", 0))
        {
            PlayerPrefs.SetInt("HighScore", score);
        }
        
        _scoreText.text = score.ToString();
    }
    
    public void ResetScore()
    {
        lastScore = score;
        score = 0;
    }
}