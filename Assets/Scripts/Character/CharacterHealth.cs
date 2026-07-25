using UnityEngine;

public class CharacterHealth : MonoBehaviour
{
    [SerializeField] private Character ownerCharacter;

    [Header("Health Chracter Config")]
    [SerializeField] private float maxHealth;
    [SerializeField] private float currentHealth;
    [SerializeField] private HealthUI healthUI;

    #region Event System
    private void OnEnable()
    {
        GlobalEvent.OnKillEnemy.AddListener(TakeHealing);
    }

    private void OnDisable()
    {
        OnRemoveListener();
    }

    private void OnDestroy()
    {
        OnRemoveListener();
    }

    private void OnRemoveListener()
    {
        GlobalEvent.OnKillEnemy.RemoveListener(TakeHealing);

    }

    #endregion

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

        if (ownerCharacter.CharacterType == CharacterType.Player)
            SoundEffectManager.Instance.PlaySound2D("player_hurt");

        healthUI.UpdateHralthSlider(currentHealth, maxHealth);
        ownerCharacter.CharacterVisualizer.PlayVfx("Blip");
    }

    public void TakeHealing()
    {
        currentHealth += 5;
        healthUI.UpdateHralthSlider(currentHealth, maxHealth);

    }
}
