using UnityEngine;

public abstract class CombatStyle : MonoBehaviour
{
    public abstract void OnUsingCombatStyle(EnemyCharacter character);

    public abstract void OnResetCombatStyle();
}
