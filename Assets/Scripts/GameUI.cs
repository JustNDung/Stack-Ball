using UnityEngine;
using UnityEngine.UI;

public class GameUI : MonoBehaviour
{
    public GameObject homeUI, inGameUI;
    public GameObject allButtons;

    private bool _buttons;
    [Header("InGame")] 
    public Image levelSlider;
    public Image currentLevelImg;
    public Image nextLevelImg;

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
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && _ball.ballState == Ball.BallState.Prepare)
        {
            _ball.ballState = Ball.BallState.Playing;
            homeUI.SetActive(false);
            inGameUI.SetActive(true);
        }
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