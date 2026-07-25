using UnityEngine;
using System.Collections;

public class EnemyCombatCharacter : CharacterCombat
{
    [SerializeField] private float coolDownBasic;
    [SerializeField] private float invtervalBetween;
    [SerializeField] private int limitBasicAtk;

    private float _interval;
    private int _currBasicAtk;
    [SerializeField] private bool isCoolDown;
    private Transform _currentPlayerPos;

    public void SetPlayerPosition(Transform playerPosition)
    {
        if (playerPosition == null)
        {
            Debug.LogWarning($"{gameObject.name} cannot find player");
            return;
        }

        _currBasicAtk = 0;
        _interval = 0f;
        isCoolDown = false;

        _currentPlayerPos = playerPosition;
    }

    public override void OnAttackByState()
    {
        if (ownerCharacter == null)
        {
            Debug.Log($"{ownerCharacter.name} is cannot find player Transform");
            return;
        }

        if (_currentPlayerPos == null)
            return;

        Vector2 dirToPlayer = (_currentPlayerPos.position - ownerCharacter.transform.position).normalized;
        ownerCharacter.CharacterObjectRotation.RotateIndicator(dirToPlayer);

        if (isCoolDown) return;

        if (_currentPlayerPos == null)
            return;

        // Eksekusi Serangan
        if (_interval >= invtervalBetween)
        {
            ownerCharacter.CharacterCombat.OnAttack(_currentPlayerPos);
            _currBasicAtk++;
            _interval = 0f; // Reset interval setelah menembak
            if (_currBasicAtk >= limitBasicAtk)
            {

                isCoolDown = true;
                ownerCharacter.StartCoroutine(OnCoolDownRoutine());
            }
        }
        else
        {
            _interval += Time.deltaTime;
        }
    }

    public override void OnAttack(Transform directionAttack)
    {
        if (pointer == null || directionAttack == null) return;

        // Hitung arah lurus dari moncong/pointer musuh ke posisi player SAAT INI
        Vector2 direction = (directionAttack.position - pointer.position).normalized;

        Projectile newBullet = _bulletPool.Get();

        // Pindahkan peluru ke titik moncong (pointer) musuh
        newBullet.transform.position = pointer.position;

        // Inisialisasi peluru agar terbang searah 'direction'
        newBullet.Init(directionAttack.position, direction);
    }

    public void ResetCombat()
    {
        _currBasicAtk = 0;
        _interval = 0f;
        isCoolDown = false;
    }

    private IEnumerator OnCoolDownRoutine()
    {
        isCoolDown = true;

        yield return new WaitForSeconds(coolDownBasic);
        _currBasicAtk = 0;
        _interval = 0f;
        isCoolDown = false;

    }
}
