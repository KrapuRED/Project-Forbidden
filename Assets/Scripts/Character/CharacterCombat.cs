using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterCombat : MonoBehaviour
{
    [SerializeField] private Character ownerCharacter;

    [Header("Combat Config")]
    [SerializeField] private Bullet bulletPrefab;

    [Header("References")]
    [SerializeField] private PlayerInput playerInput;
    [SerializeField] private Transform pointSpawn;

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
            Debug.Log($"{gameObject.name} is missing playerInput!");
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

        OnAttack();
    }


    public void OnAttack()
    {
        Bullet newBullet = Instantiate(bulletPrefab, pointSpawn.position, Quaternion.identity);
        newBullet.Init(pointSpawn.position);
    }
}
