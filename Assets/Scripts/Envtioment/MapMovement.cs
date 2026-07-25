using UnityEngine;

public class MapMovement : MonoBehaviour
{
    [Header("Map Movement Config")]
    [SerializeField] private float speedMapMovement;

    private void Update()
    {
        transform.Translate(Vector2.down * speedMapMovement * Time.deltaTime);
    }
}
