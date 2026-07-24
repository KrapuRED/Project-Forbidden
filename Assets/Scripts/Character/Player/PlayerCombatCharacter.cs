using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCombatCharacter : CharacterCombat
{
    [Header("Player Combat Character References")]
    [SerializeField] protected PlayerInput playerInput;

    #region Event System
    private void OnEnable()
    {
        if (playerInput != null)
        {
            playerInput.actions["Combat"].performed += OnInputAttack;
            playerInput.actions["Combat"].canceled += OnInputAttack;

        }
    }

    private void OnDisable()
    {
        OnRemoveListener();
    }

    private void OnDestroy()
    {
        OnRemoveListener();
    }

    private void OnRemoveListener()
    {
        if (playerInput == null)
        {
            Debug.LogWarning($"{gameObject.name} is missing playerInput!");
            return;
        }

        playerInput.actions["Combat"].performed -= OnInputAttack;
        playerInput.actions["Combat"].canceled -= OnInputAttack;
    }
    #endregion

    public void OnInputAttack(InputAction.CallbackContext context)
    {
        if (!context.performed)
            return;

        OnAttack(pointer);
    }


    public override void OnAttack(Transform directionAttack)
    {
        Vector2 direction = pointer.up;

        Projectile newBullet = _bulletPool.Get();
        newBullet.Init(directionAttack.position, direction);
    }
}
