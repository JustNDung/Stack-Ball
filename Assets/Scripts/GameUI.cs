using UnityEngine;
using UnityEngine.UI;

public class GameUI : MonoBehaviour
{
    [Header("InGame")] 
    public Image levelSlider;
    public Image currentLevelImg;
    public Image nextLevelImg;

    private Material _ballMat;

    private void Awake()
    {
        _ballMat = FindFirstObjectByType<Ball>().transform.GetChild(0).GetComponent<MeshRenderer>().material;
        levelSlider.transform.parent.GetComponent<Image>().color = _ballMat.color + Color.gray;
        levelSlider.color = _ballMat.color;
        currentLevelImg.color = _ballMat.color;
        nextLevelImg.color = _ballMat.color;
    }
    
    public void LevelSliderFill(float value)
    {
        levelSlider.fillAmount = value;
    }
    
    
}