using UnityEngine;

public class CharacterHealth : MonoBehaviour
{
    [SerializeField] private Character ownerCharacter;

    [Header("Health Chracter Config")]
    [SerializeField] private float maxHealth;
    [SerializeField] private float currentHealth;

    private void Start()
    {
        currentHealth = maxHealth;
    }

    public void Init(float deafaultHealth)
    {
        currentHealth = maxHealth = deafaultHealth;
    }

    public void TakingDamage(float damageValue)
    {

    }
}
