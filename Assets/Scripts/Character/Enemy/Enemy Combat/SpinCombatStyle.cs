using UnityEngine;
using System.Collections;

public class SpinCombatStyle : CombatStyle
{
    [SerializeField] private float coolDown;
    [SerializeField] private bool isCoolDown;
    [SerializeField] private GameObject smallBulletPrefab;

    [Header("Spin Combat Style Config")]
    [SerializeField] private int attackDirection;
    [SerializeField] private float spreadAngle;
    [SerializeField] private float smallBulletSpeed;
    [SerializeField] private float burstInterval;
    [SerializeField] private int maxBurstCount = 1;


    public override void OnResetCombatStyle()
    {
        StopCoroutine(CoolDown());

        isCoolDown = false;
    }

    public override void OnUsingCombatStyle(EnemyCharacter character)
    {
        if (isCoolDown)
        {
            return;
        }

        EnemyCombatCharacter enemyCombat = character.EnemyCombatCharacter;

        Transform target = enemyCombat.CurrentPLayerPosition;
        if (target == null)
            return;

        Projectile mainBullet = enemyCombat.FireMainBullet(target);
        if (mainBullet == null) return;

        BulletBurstEmitter emitter = mainBullet.GetComponent<BulletBurstEmitter>();
        if (emitter == null)
            emitter = mainBullet.gameObject.AddComponent<BulletBurstEmitter>();

        emitter.Configure(
            smallBulletPrefab: smallBulletPrefab,
            burstCount: attackDirection,
            spreadAngle: spreadAngle,
            smallBulletSpeed: smallBulletSpeed,
            burstInterval: burstInterval,
            maxBurstCount: maxBurstCount);

        StartCoroutine(CoolDown());
    }

    private IEnumerator CoolDown()
    {
        isCoolDown = true;

        yield return new WaitForSeconds(coolDown);
        isCoolDown = false;
    }
}
