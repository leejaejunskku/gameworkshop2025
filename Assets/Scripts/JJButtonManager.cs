using UnityEngine;
using UnityEngine.UI; // Button 컴포넌트 사용을 위해 추가

public class JJButtonManager : MonoBehaviour
{
    
    public Button startButton;
    public Button tutorialButton;
    public Button tutorialCloseButton;
    public SpriteRenderer tutorialSprite;

    public void ShowMainButtons()
    {
        startButton.gameObject.SetActive(true);
        tutorialButton.gameObject.SetActive(true);
        tutorialCloseButton.gameObject.SetActive(false);
        tutorialSprite.gameObject.SetActive(false);
    }

    public void ShowTutorialMode()
    {
        startButton.gameObject.SetActive(false);
        tutorialButton.gameObject.SetActive(false);
        tutorialCloseButton.gameObject.SetActive(true);
        tutorialSprite.gameObject.SetActive(true);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ShowMainButtons(); // 시작할 때 메인 버튼들 표시
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

