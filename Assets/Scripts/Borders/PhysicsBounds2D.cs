using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PhysicsBounds2D : MonoBehaviour
{
    [SerializeField] private Character ownerCharacter;

    [Header("Assign Bounds here")]
    [SerializeField] private Collider2D moveBounds;

    private Rigidbody2D rb2d;

    private void Awake()
    {
        rb2d = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        if (moveBounds == null) return;

        Vector2 currentPos = rb2d.position;
        Vector2 clampedPos = moveBounds.ClosestPoint(currentPos);

        if (Vector2.Distance(currentPos,clampedPos) > 0.01f) 
        {
            rb2d.position = clampedPos;

            Vector2 inputDir = ownerCharacter.CharacterMovement.OwnerDirection;
            Vector2 directionToInside = (clampedPos - currentPos).normalized;

            if (Vector2.Dot(inputDir, directionToInside) <= 0)
            {
                ownerCharacter.CharacterMovement.StopAtBorder();
            }
        }
    }
}
