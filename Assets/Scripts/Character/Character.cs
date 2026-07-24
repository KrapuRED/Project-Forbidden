using UnityEngine;

public class Character : MonoBehaviour
{
    [Header("== Chatacter System ==")]
    [SerializeField] private CharacterMovement chatacterMovement;
    [SerializeField] private CharacterHealth  characterHealth;
    [SerializeField] private CharacterCombat characterCombat;

    public CharacterMovement CharacterMovement => chatacterMovement;
    public CharacterHealth CharacterHealth => characterHealth;
    public CharacterCombat CharacterCombat => characterCombat;

    public virtual void OnDeathCharacter()
    {

    }
}
