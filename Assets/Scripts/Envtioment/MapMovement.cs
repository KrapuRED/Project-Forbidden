using UnityEngine;

public class MapMovement : MonoBehaviour
{
    [Header("Map Movement Config")]
    [SerializeField] private float speedMapMovement;

    private void Update()
    {
        if (!GameManager.Instance.IsGameActive)
            return;

        transform.Translate(Vector2.down * speedMapMovement * Time.deltaTime);
    }
}
