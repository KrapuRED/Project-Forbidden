using UnityEngine;
using System.Collections;

public class EnemyCombatCharacter : CharacterCombat
{
    [SerializeField] private EnemyCharacter enemyCharacter;
    [SerializeField] private CombatStyle combatStyle;

    private Transform _currentPlayerPos;
    public Transform CurrentPLayerPosition => _currentPlayerPos;

    public void SetPlayerPosition(Transform playerPosition)
    {
        if (playerPosition == null)
        {
            Debug.LogWarning($"{gameObject.name} cannot find player");
            return;
        }

        _currentPlayerPos = playerPosition;
    }

    public override void OnAttackByState()
    {
        combatStyle.OnUsingCombatStyle(enemyCharacter);
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

    public Projectile FireMainBullet(Transform target)
    {
        if (pointer == null || target == null) return null;

        Vector2 direction = (target.position - pointer.position).normalized;
        Projectile newBullet = _bulletPool.Get();
        newBullet.transform.position = pointer.position;
        newBullet.Init(target.position, direction);
        return newBullet;
    }

    public void ResetCombat() => combatStyle.OnResetCombatStyle();
}
