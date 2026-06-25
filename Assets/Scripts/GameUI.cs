using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GameUI : MonoBehaviour
{
    public GameObject homeUI, inGameUI, finishUI, gameOverUI;
    public GameObject allButtons;

    private bool _buttons;

    [Header("PreGame")] 
    public Button soundButton;
    public Sprite soundOnS, soundOffS;
    
    [Header("InGame")] 
    public Image levelSlider;
    public Image currentLevelImg;
    public Image nextLevelImg;
    public Text currentLevelText, nextLevelText;

    [Header("Finish")] 
    public Text finishLevelText;

    [Header("GameOver")] 
    public Text gameOverScoreText;
    public Text gameOverBestScoreText;

    private Material _ballMat;
    private Ball _ball;

    private void Awake()
    {
        _ballMat = FindFirstObjectByType<Ball>().transform.GetChild(0).GetComponent<MeshRenderer>().material;
        _ball = FindFirstObjectByType<Ball>();
        
        levelSlider.transform.parent.GetComponent<Image>().color = _ballMat.color + Color.gray;
        levelSlider.color = _ballMat.color;
        currentLevelImg.color = _ballMat.color;
        nextLevelImg.color = _ballMat.color;
        
        soundButton.onClick.AddListener(() => SoundManager.Instance.SoundOnOff());
    }

    private void Start()
    {
        currentLevelText.text = FindFirstObjectByType<LevelSpawner>().level.ToString();
        nextLevelText.text = (FindFirstObjectByType<LevelSpawner>().level + 1).ToString();
    }

    private void Update()
    {
        if (_ball.ballState == Ball.BallState.Prepare)
        {
            if (SoundManager.Instance.sound && soundButton.GetComponent<Image>().sprite != soundOnS)
            {
                soundButton.GetComponent<Image>().sprite = soundOnS;
            }
            else if (!SoundManager.Instance.sound && soundButton.GetComponent<Image>().sprite != soundOffS)
            {
                soundButton.GetComponent<Image>().sprite = soundOffS;
            }
        }
        
        if (Input.GetMouseButtonDown(0) && !IgnoreUI() && _ball.ballState == Ball.BallState.Prepare)
        {
            _ball.ballState = Ball.BallState.Playing;
            homeUI.SetActive(false);
            inGameUI.SetActive(true);
            finishUI.SetActive(false);
            gameOverUI.SetActive(false);
        }

        if (_ball.ballState == Ball.BallState.Finish)
        {
            inGameUI.SetActive(false);
            finishUI.SetActive(true);
            gameOverUI.SetActive(false);
            finishLevelText.text = "LEVEL " + FindFirstObjectByType<LevelSpawner>().level + " COMPLETE!";
        }
        
        if (_ball.ballState == Ball.BallState.Died)
        {
            inGameUI.SetActive(false);
            finishUI.SetActive(false);
            gameOverUI.SetActive(true);
            gameOverScoreText.text = "Score: " + ScoreManager.Instance.lastScore;
            gameOverBestScoreText.text = "Best: " + PlayerPrefs.GetInt("HighScore", 0);
        }
    }

    private bool IgnoreUI()
    {
        PointerEventData pointerEventData = new PointerEventData(EventSystem.current);
        pointerEventData.position = Input.mousePosition;
        
        List<RaycastResult> raycastResults = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointerEventData, raycastResults);

        for (int i = 0; i < raycastResults.Count; i++)
        {
            if (raycastResults[i].gameObject.GetComponent<Ignore>() != null)
            {
                raycastResults.RemoveAt(i);
                i--;
            } 
        }
        return raycastResults.Count > 0;
    }
    
    public void LevelSliderFill(float value)
    {
        levelSlider.fillAmount = value;
    }

    public void Settings()
    {
        _buttons = !_buttons;
        allButtons.SetActive(_buttons);
    }
    
    
}