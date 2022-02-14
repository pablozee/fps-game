using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerStats : MonoBehaviour
{
    [SerializeField] private float maxHealth;
    [SerializeField] private Image damageOverlay;
    [SerializeField] private CameraShake cameraShake;
    [SerializeField] private float cameraShakeDuration;
    [SerializeField] private float cameraShakeMagnitude;
    [SerializeField] private HealthBar healthBar;
    [SerializeField] private AudioClip[] audioClips;
    [SerializeField] private AudioSource hurtAudioSource;

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
        healthBar.SetCurrentHealth(currentHealth);
    }

    public void TakeDamage(float amount)
    {
        currentHealth -= amount;

      //  hurtAudioSource.clip = audioClips[Random.Range(0, audioClips.Length)];
       // hurtAudioSource.Play();

        ShowDamageOverlay();


        cameraShake.StartCoroutine(cameraShake.Shake(cameraShakeDuration, cameraShakeMagnitude));


        healthBar.SetCurrentHealth(currentHealth);

        Debug.Log("Player took " + amount + " damage.");

        if (currentHealth <= 0)
        {
            // Game over
            Debug.Log("Game over. Player died.");
            Die();
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

    void Die()
    {
        SceneManager.LoadScene("GameOver");
    }
}
