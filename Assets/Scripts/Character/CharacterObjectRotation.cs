using UnityEngine;
using UnityEngine.InputSystem;


public class CharacterObjectRotation : MonoBehaviour
{
    [SerializeField] private Character ownerCharacter;
    [Header("AI Setting")]
    [SerializeField]    private bool isAIControlled;

    [Header("Pointer")]
    [SerializeField] private Transform pointer;
    [SerializeField] private float distanceView;
    [SerializeField] private float offsetPointer;
    private Transform _ownerTransform;
    private Vector2 _mouseScreenPosition;

    [Header("References")]
    [SerializeField] private PlayerInput playerInput;

    private Camera _cam;

    private void Awake()
    {
        _cam = Camera.main;
        _ownerTransform = ownerCharacter.transform;
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
        if (playerInput != null)
        {
            playerInput.actions["MousePosition"].performed -= ChangePositionPointer;
            playerInput.actions["MousePosition"].canceled -= ChangePositionPointer;
        }
    }

    #endregion

    private void Update()
    {
        if (!GamaManager.Instance.IsGameActive)
            return;

        if (isAIControlled)
            return;

        if (_cam == null || pointer == null || _ownerTransform == null)
            return;

        Vector3 mouseWorldPos = _cam.ScreenToWorldPoint(
            new Vector3(_mouseScreenPosition.x, _mouseScreenPosition.y, _cam.transform.position.z * -1f)
        );
        mouseWorldPos.z = 0f;

        Vector2 direction = (mouseWorldPos - _ownerTransform.position).normalized;

        RotateIndicator(direction);
        RotateSprite(direction);
    }

    private void ChangePositionPointer(InputAction.CallbackContext contex)
    {
        _mouseScreenPosition = contex.ReadValue<Vector2>();
    }

    public void RotateIndicator(Vector2 direction)
    {
        if (pointer == null || _ownerTransform == null) return;

        // Position pointer at offset along that direction
        pointer.position = (Vector2)_ownerTransform.position + direction * offsetPointer;

        // Rotate pointer to face the target/cursor
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        pointer.rotation = Quaternion.Euler(0f, 0f, angle - 90f);
    }

    public void RotateSprite(Vector2 direction)
    {
        if (ownerCharacter == null)
        {
            Debug.LogWarning($"{gameObject.name} is missing Owner Character!");
            return;
        }

        pointer.position = (Vector2)_ownerTransform.position + direction * offsetPointer;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        ownerCharacter.transform.rotation = Quaternion.Euler(0f, 0f, angle - 90f);
    }

    private void OnDrawGizmos()
    {
        if(pointer == null) return;

        // Warna garis penunjuk
        Gizmos.color = Color.green;

        // Arah tembakan indikator (menggunakan pointer.up karena rotasi di-offset -90 degree)
        Vector3 direction = pointer.up;

        // Titik akhir garis (Titik awal + (Arah * Jarak))
        Vector3 endPosition = pointer.position + (direction * distanceView);

        // Gambar garis dari pointer ke titik akhir
        Gizmos.DrawLine(pointer.position, endPosition);

        // Opsional: Gambar bola kecil di ujung garis sebagai penanda target/range maksimum
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(endPosition, 0.2f);

    }
}
