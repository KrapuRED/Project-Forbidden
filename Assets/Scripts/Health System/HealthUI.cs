using UnityEngine;
using UnityEngine.UI;

public class HealthUI : MonoBehaviour
{
    [SerializeField] private Slider healthSlider;

    public void UpdateHralthSlider(float currentHealth, float maxHealth)
    {
        if (healthSlider == null)
            return;

        healthSlider.value = currentHealth / maxHealth;
    }
}
