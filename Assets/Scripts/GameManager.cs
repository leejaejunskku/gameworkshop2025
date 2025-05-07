using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    public GameObject gameoverText;
    public Text timeText;
    public Text recordText;

    private float surviveTime;
    private bool isGameOver;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
//        //게임이 시작할 때 게임오버 텍스트를 비활성화
        surviveTime = 0.0f;
        isGameOver = false;

    }

    // Update is called once per frame
    void Update()
    {
        if (!isGameOver)
        {
            //게임이 진행중일 때
            surviveTime += Time.deltaTime;
            timeText.text = "Survive Time: " + Mathf.FloorToInt(surviveTime) + "s";
            
        }
        else
        {
            
            if (Input.GetKeyDown(KeyCode.R))
            {
                SceneManager.LoadScene("2DStage");
            }
        }
        
    }

    public void EndGame()
    {
        //게임이 종료되었을 때 isGameover에 true를 넣습니다.
        isGameOver = true;
        
        gameoverText.SetActive(true);
        
        float bestTime = PlayerPrefs.GetFloat("BestTime");

        if (surviveTime > bestTime)
        {
            //최고 기록 값을 생존 시간 값으로 변경
            bestTime = surviveTime;
            PlayerPrefs.SetFloat("BestTime", bestTime);
            
        }
        
        recordText.text = "Best Time: " + Mathf.FloorToInt(bestTime) + "s";
    }
}
