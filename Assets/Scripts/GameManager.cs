using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    private bool isGameActive = true;

    public bool IsGameActive => isGameActive;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        MusicManager.Instance.PlayMusic("Game");
    }

    public void PlayGame()
    {
        isGameActive = true;
        TransitionManager.Instance.OnLoadRandomTransition("GamePlay_MainGame");
    }

    public void GameOver()
    {
        isGameActive = false;

        PanelManager.Instance.OpenPanel("Panel - Failed");
    }

    public void EndGame()
    {
        isGameActive = false;
        TransitionManager.Instance.OnLoadScene("Credit", "CrossFade");
    }

    public void MainMenu()
    {
        PanelManager.Instance.ClosePanel("Panel - Failed");
        isGameActive = false;
        TransitionManager.Instance.OnLoadRandomTransition("GamePlay_MainMenu");
    }

    public void RestartGame()
    {
        PanelManager.Instance.ClosePanel("Panel - Failed");
        isGameActive = true;


        TransitionManager.Instance.OnLoadRandomTransition("GamePlay_MainGame");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
