using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public void PlayGame()
    {
        GameManager.Instance.PlayGame();
    }

    public void QuitGame()
    {
        GameManager.Instance.QuitGame();
    }
}
