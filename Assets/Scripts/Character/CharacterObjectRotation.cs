using UnityEngine;
using UnityEngine.InputSystem;


public class CharacterObjectRotation : MonoBehaviour
{
    [SerializeField] private Character ownerCharacter;

    [Header("Pointer")]
    [SerializeField] private Transform pointer;
    [SerializeField] private float offsetPointer;
    private Transform _playerTransform;
    private Vector2 _mouseScreenPosition;

    [Header("References")]
    [SerializeField] private PlayerInput playerInput;

    private Camera _cam;

    private void Awake()
    {
        _cam = Camera.main;
        _playerTransform = ownerCharacter.transform;
    }

    #region Event System
    private void OnEnable()
    {
        if (playerInput != null)
        {
            playerInput.actions["MousePosition"].performed += ChangePositionPointer;
            playerInput.actions["MousePosition"].canceled += ChangePositionPointer;

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

        playerInput.actions["MousePosition"].performed -= ChangePositionPointer;
        playerInput.actions["MousePosition"].canceled -= ChangePositionPointer;
    }

    #endregion

    private void Update()
    {
        if (_cam == null || pointer == null || _playerTransform == null)
            return;

        // convert mouse screen position -> world position
        Vector3 mouseWorldPos = _cam.ScreenToWorldPoint(
            new Vector3(_mouseScreenPosition.x, _mouseScreenPosition.y, _cam.transform.position.z * -1f)
        );
        mouseWorldPos.z = 0f;

        // direction from player to cursor
        Vector2 direction = (mouseWorldPos - _playerTransform.position).normalized;

        // position pointer at offset along that direction
        pointer.position = (Vector2)_playerTransform.position + direction * offsetPointer;

        // rotate pointer to face the cursor
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        pointer.rotation = Quaternion.Euler(0f, 0f, angle - 90f);
    }

    private void ChangePositionPointer(InputAction.CallbackContext contex)
    {
        _mouseScreenPosition = contex.ReadValue<Vector2>();
    }
}
