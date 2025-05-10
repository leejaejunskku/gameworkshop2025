using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class JJSceneManager : MonoBehaviour
{
    private static JJSceneManager instance;
    public static JJSceneManager Instance
    {
        get { return instance; }
    }

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private Stack<SceneType> sceneHistory = new Stack<SceneType>();
    
    [SerializeField]
    private JJButtonManager buttonManager;

    void Start()
    {
        // 현재 씬을 히스토리에 추가
        sceneHistory.Push(GetCurrentSceneType());
    }

    void Update()
    {
        // Android 백 버튼 처리
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            GoToPreviousScene();
        }
    }

    private SceneType GetCurrentSceneType()
    {
        string sceneName = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
        return System.Enum.Parse<SceneType>(sceneName);
    }

    public void LoadScene(SceneType sceneType)
    {
        sceneHistory.Push(sceneType);
        UnityEngine.SceneManagement.SceneManager.LoadScene(sceneType.ToString());
    }

    public void GoToPreviousScene()
    {
        if (sceneHistory.Count > 1)
        {
            sceneHistory.Pop(); // 현재 씬 제거
            SceneType previousScene = sceneHistory.Peek();
            UnityEngine.SceneManagement.SceneManager.LoadScene(previousScene.ToString());
        }
        else
        {
            // 메인 메뉴에서는 게임 종료
            Application.Quit();
        }
    }

    // UI 버튼 이벤트 핸들러
    public void OnStartButtonClick()
    {
        LoadScene(SceneType.Stage2D);
    }

    public void OnTutorialButtonClick()
    {
        buttonManager.ShowTutorialMode();
    }

    public void OnTutorialCloseButtonClick()
    {
        buttonManager.ShowMainButtons();
    }
    
    public void LoadStartScene()
    {
        LoadScene(SceneType.Stage2D);
    }
}
