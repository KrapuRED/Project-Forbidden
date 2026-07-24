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

    public void OnTakingDamage(float damageValue)
    {
        Debug.Log($"{gameObject.name} is taking damage {damageValue}");

        currentHealth = Mathf.Min(currentHealth - damageValue, maxHealth);

        if (currentHealth <= 0)
        {
            ownerCharacter.OnDeathCharacter();
            return;
        }
    }
}
