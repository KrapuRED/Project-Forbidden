using UnityEngine;

public class EnemyCombatCharacter : CharacterCombat
{
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
}
