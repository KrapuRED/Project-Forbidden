using UnityEngine;

public abstract class StateSO : ScriptableObject
{
    public abstract void EnterState(EnemyCharacter character);

    public abstract void ExcuteState(EnemyCharacter character);

    public abstract void ExitState(EnemyCharacter character);
}
