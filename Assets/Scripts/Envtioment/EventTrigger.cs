using UnityEngine;

public class EventTrigger : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        EnemySpawner.Instance.OnSpawningEnemy();
    }
}
