using UnityEngine;

public class GamaManager : MonoBehaviour
{
    public static GamaManager Instance { get; private set; }

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
    }

    public void GameOver()
    {
        isGameActive = false;
    }

    public void EndGame()
    {
        isGameActive = false;
    }

    public void RestartGame()
    {

    }

    public void QuitGame()
    {

    }
}
