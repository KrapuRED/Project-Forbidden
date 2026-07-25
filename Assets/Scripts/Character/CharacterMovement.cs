using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterMovement : MonoBehaviour
{
    [Header("Movement & Speed Config")]
    [SerializeField] private float moveSpeed;

    [Header("References")]
    [SerializeField] private Character ownerMovement;
    [SerializeField] private PlayerInput playerInput;

    private Vector2 _ownerDirection;
    private Rigidbody2D _rb2d;
    private bool isReady;

    public Vector2 OwnerDirection => _ownerDirection;

    #region Event System
    private void OnEnable()
    {
        if (playerInput != null)
        {
            playerInput.actions["Movement"].performed += OnInputMovement;
            playerInput.actions["Movement"].canceled += OnInputMovement;
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
            playerInput.actions["Movement"].performed -= OnInputMovement;
            playerInput.actions["Movement"].canceled -= OnInputMovement;
        }
    }

    #endregion

    private void Awake()
    {
        if (_rb2d == null)
            _rb2d = ownerMovement.GetComponent<Rigidbody2D>();

        isReady = _rb2d ? true : false;
    }

    public void OnInputMovement(InputAction.CallbackContext contex)
    {
        if (this == null) return; // catches destroyed-but-still-invoked case

        _ownerDirection = contex.ReadValue<Vector2>();
        OnMoveCharacter(_ownerDirection);
    }

    public void OnMoveCharacter(Vector2 direction)
    {
        if (!GameManager.Instance.IsGameActive)
            return;
        if (_rb2d == null) // check the real thing, not a stale bool
        {
            Debug.Log($"{gameObject.name} is missing RigidBody2D!");
            return;
        }
        float movementSpeed = direction.magnitude;
        if (ownerMovement != null && ownerMovement.CharacterAnimation != null)
            ownerMovement.CharacterAnimation.PlayWalkingAnimtion(movementSpeed);

        _rb2d.linearVelocity = direction * moveSpeed;
    }

    public void OnMoveCharacterByPath(Vector2 targetPosition)
    {
        if (_rb2d != null)
        {
            _rb2d.MovePosition(targetPosition);
        }
        else
        {
            ownerMovement.transform.position = targetPosition;
        }
    }

    public void StopAtBorder()
    {
        _rb2d.linearVelocity = Vector2.zero;
    }
}
