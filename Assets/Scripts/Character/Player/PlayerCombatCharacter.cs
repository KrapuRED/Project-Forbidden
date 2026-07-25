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
        if (playerInput != null)
        {
            playerInput.actions["Combat"].performed -= OnInputAttack;
            playerInput.actions["Combat"].canceled -= OnInputAttack;
        }
        
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
        if (!GameManager.Instance.IsGameActive)
            return;

        if (directionAttack == null) // Unity's overloaded == catches destroyed objects too
            return;

        Vector2 direction = pointer.up; // note: see below, this should probably use directionAttack
        SoundEffectManager.Instance.PlaySound2D("player_shoot");
        Projectile newBullet = _bulletPool.Get();
        newBullet.Init(directionAttack.position, direction);
    }

    public override void OnAttackByState()
    {
        
    }
}
