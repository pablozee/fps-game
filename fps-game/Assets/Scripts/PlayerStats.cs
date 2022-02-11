using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStats : MonoBehaviour
{
    [SerializeField] private float maxHealth;
    [SerializeField] private Image damageOverlay;
    [SerializeField] private CameraShake cameraShake;
    [SerializeField] private float cameraShakeDuration;
    [SerializeField] private float cameraShakeMagnitude;
    [SerializeField] private HealthBar healthBar;

    private float currentHealth;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
    }

    // Update is called once per frame
    void Update()
    {
        FadeOutDamageOverlay();
    }

    public void TakeDamage(float amount)
    {
        currentHealth -= amount;

        healthBar.SetCurrentHealth(currentHealth);

        ShowDamageOverlay();


        cameraShake.StartCoroutine(cameraShake.Shake(cameraShakeDuration, cameraShakeMagnitude));

        Debug.Log("Player took " + amount + " damage.");

        if (currentHealth <= 0)
        {
            // Game over
            Debug.Log("Game over. Player died.");
        }
    }

    void ShowDamageOverlay()
    {
        Color color = damageOverlay.color;
        color.a = 0.8f;
        damageOverlay.color = color;
    }

    void FadeOutDamageOverlay()
    {
        if (damageOverlay != null)
        {
            if (damageOverlay.color.a <= 0) return;

            Color color = damageOverlay.color;
            color.a -= 0.01f;
            damageOverlay.color = color;
        }
    }
}
