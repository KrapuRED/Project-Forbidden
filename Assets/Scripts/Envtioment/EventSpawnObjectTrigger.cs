using UnityEngine;

public class EventSpawnObjectTrigger : MonoBehaviour
{
    [SerializeField] private float speedEnemy;
    [SerializeField] private int minEnemy;
    [SerializeField] private int maxEnemy;

    [SerializeField] private bool isBeenTrigger;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player"))
            return;

        if (isBeenTrigger)
            return;

        isBeenTrigger = true;
        int randomSpawned = Random.Range(minEnemy, maxEnemy);

        EnemySpawner.Instance.StartSpawning(speedEnemy, randomSpawned);
    }
}
