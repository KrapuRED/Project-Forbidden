using UnityEngine;

public class CharacterHealth : MonoBehaviour
{
    [SerializeField] private Character ownerCharacter;

    [Header("Health Chracter Config")]
    [SerializeField] private float maxHealth;
    [SerializeField] private float currentHealth;
    [SerializeField] private HealthUI healthUI;

    private void Start()
    {
        currentHealth = maxHealth;
        healthUI.UpdateHralthSlider(currentHealth, maxHealth);

    }

    public void Init(float deafaultHealth)
    {
        currentHealth = maxHealth = deafaultHealth;
        healthUI.UpdateHralthSlider(currentHealth, maxHealth);
    }

    public void OnTakingDamage(float damageValue)
    {
        currentHealth = Mathf.Min(currentHealth - damageValue, maxHealth);

        if (currentHealth <= 0)
        {
            ownerCharacter.OnDeathCharacter();
            return;
        }

        healthUI.UpdateHralthSlider(currentHealth, maxHealth);
    }
}
