using UnityEngine;

public class EnemyCharacter : Character
{
    public override void OnDeathCharacter()
    {
        Debug.Log($"{gameObject.name} is Dead");
        EntityCounterManager.Instance.RemoveEntityFormCounterByID(EntityID);
        Destroy(gameObject);
    }
}
