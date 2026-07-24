using UnityEngine;
using System.Collections;

[CreateAssetMenu(fileName = "StateAttackPlayerSO", menuName = "State Machine/States/StateAttackPlayerSO")]
public class StateAttackPlayerSO : StateSO
{
    [SerializeField] private float coolDownBasic;
    [SerializeField] private float invtervalBetween;
    [SerializeField] private int limitBasicAtk;

    private float _interval;
    private int _currBasicAtk;
    private bool isCoolDown;
    private Transform _currentPlayerPos;

    public override void EnterState(Character character)
    {
        Transform playerPos = EntityCounterManager.Instance.GetEntityByID("PL")?.entityPosition;

        if (playerPos == null)
        {
            Debug.LogWarning($"[{character.name}] Gagal menemukan Player Position!");
            return;
        }

        _currBasicAtk = 0;
        _interval = 0f;
        isCoolDown = false;
        _currentPlayerPos = playerPos;
    }

    public override void ExcuteState(Character character)
    {
        if (character == null)
        {
            Debug.Log($"{character.name} is cannot find player Transform");
            return;
        }

        Vector2 dirToPlayer = (_currentPlayerPos.position - character.transform.position).normalized;
        character.CharacterObjectRotation.RotateIndicator(dirToPlayer);

        if (isCoolDown) return;

        if (_currentPlayerPos == null)
            return;

        // Eksekusi Serangan
        if (_interval >= invtervalBetween)
        {
            character.CharacterCombat.OnAttack(_currentPlayerPos);
            _currBasicAtk++;
            _interval = 0f; // Reset interval setelah menembak
            if (_currBasicAtk >= limitBasicAtk)
            {
                // Ubah flag SEKETIKA agar frame berikutnya tidak lolos
                isCoolDown = true;
                character.StartCoroutine(OnCoolDownRoutine());
            }
        }
        else
        {
            _interval += Time.deltaTime;
        }
    }

    public override void ExitState(Character character)
    {
        _currentPlayerPos = null;
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
