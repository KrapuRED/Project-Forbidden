using UnityEngine;
using System.Collections;

public class BurstCombatStyle : CombatStyle
{
    [SerializeField] private float coolDownBasic;
    [SerializeField] private float invtervalBetween;
    [SerializeField] private int limitBasicAtk;

    private float _interval;
    private int _currBasicAtk;
    [SerializeField] private bool isCoolDown;

    private Coroutine coolDownCoroutine;

    public override void OnUsingCombatStyle(EnemyCharacter character)
    {
        if (character == null)
        {
            Debug.Log($"{character.name} is cannot find player Transform");
            return;
        }

        if (character.EnemyCombatCharacter.CurrentPLayerPosition == null)
            return;

        Vector2 dirToPlayer = (character.EnemyCombatCharacter.CurrentPLayerPosition.position - character.transform.position).normalized;
        character.CharacterObjectRotation.RotateIndicator(dirToPlayer);

        if (isCoolDown) return;

        if (_interval >= invtervalBetween)
        {
            character.CharacterCombat.OnAttack(character.EnemyCombatCharacter.CurrentPLayerPosition);
            _currBasicAtk++;
            _interval = 0f; 
            if (_currBasicAtk >= limitBasicAtk)
            {
                isCoolDown = true;
                coolDownCoroutine = StartCoroutine(OnCoolDownRoutine());
            }
        }
        else
        {
            _interval += Time.deltaTime;
        }
    }

    public override void OnResetCombatStyle()
    {
        if (coolDownCoroutine != null)
            StopCoroutine(coolDownCoroutine);

        _interval = 0;
        _currBasicAtk = 0;
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
