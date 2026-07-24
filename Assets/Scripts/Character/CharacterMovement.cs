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

    private void Start()
    {
        if (_rb2d == null)
            _rb2d = ownerMovement.GetComponent<Rigidbody2D>();

        isReady = _rb2d ? true : false;
    }

    public void OnInputMovement(InputAction.CallbackContext contex)
    {
        _ownerDirection = contex.ReadValue<Vector2>();

        OnMoveCharacter(_ownerDirection);
    }

    public void OnMoveCharacter(Vector2 direction)
    {
        if (!isReady)
        {
            Debug.Log($"{gameObject.name}is missing RigidBody2D!");
            return;
        }

        _rb2d.linearVelocity = _ownerDirection * moveSpeed;
    }
}
