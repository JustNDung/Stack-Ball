using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GameUI : MonoBehaviour
{
    public GameObject homeUI, inGameUI;
    public GameObject allButtons;

    private bool _buttons;

    [Header("PreGame")] 
    public Button soundButton;
    public Sprite soundOnS, soundOffS;
    
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
        
        soundButton.onClick.AddListener(() => SoundManager.Instance.SoundOnOff());
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