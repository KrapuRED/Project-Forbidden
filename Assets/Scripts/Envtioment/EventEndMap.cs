using UnityEngine;

public class EventEndMap : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player"))
            return;

        GameManager.Instance.EndGame();
    }
}
