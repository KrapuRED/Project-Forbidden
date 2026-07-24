using UnityEngine;

[System.Serializable]
public enum CharacterType
{
    Player,
    EnemyMinion
}

public class Character : MonoBehaviour, IDamageable
{
    [SerializeField] private string entityID;
    [SerializeField] private CharacterType chracterType;

    [Header("== Chatacter System ==")]
    [SerializeField] private CharacterMovement chatacterMovement;
    [SerializeField] private CharacterObjectRotation characterObjectRotation;
    [SerializeField] private CharacterHealth  characterHealth;
    [SerializeField] private CharacterCombat characterCombat;

    public CharacterMovement CharacterMovement => chatacterMovement;
    public CharacterObjectRotation CharacterObjectRotation => characterObjectRotation;
    public CharacterHealth CharacterHealth => characterHealth;
    public CharacterCombat CharacterCombat => characterCombat;

    public CharacterType CharacterType => chracterType;
    public string EntityID => entityID;

    private void Start()
    {
        entityID = EntityCounterManager.Instance.GetEntityID(this);
    }

    public void ITakeDamage(float damageValue)
    {
        characterHealth.OnTakingDamage(damageValue);
    }

    public virtual void OnDeathCharacter()
    {

    }

}
