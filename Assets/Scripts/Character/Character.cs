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
    [SerializeField] private CharacterDataSO characterData;
    [SerializeField] private CharacterType chracterType;

    [Header("== Chatacter System ==")]
    [SerializeField] private CharacterMovement chatacterMovement;
    [SerializeField] private CharacterObjectRotation characterObjectRotation;
    [SerializeField] private CharacterHealth  characterHealth;
    [SerializeField] private CharacterCombat characterCombat;
    [SerializeField] private CharacterVisualizer characterVisualizer;

    public CharacterDataSO CharacterData => characterData;
    public CharacterMovement CharacterMovement => chatacterMovement;
    public CharacterObjectRotation CharacterObjectRotation => characterObjectRotation;
    public CharacterHealth CharacterHealth => characterHealth;
    public CharacterCombat CharacterCombat => characterCombat;
    public CharacterVisualizer CharacterVisualizer => characterVisualizer;

    public CharacterType CharacterType => chracterType;
    public string EntityID => entityID;

    public void ITakeDamage(float damageValue)
    {
        characterHealth.OnTakingDamage(damageValue);
    }

    public void SetCharacterID(string charID) => entityID = charID;

    public virtual void OnDeathCharacter()
    {

    }

}
