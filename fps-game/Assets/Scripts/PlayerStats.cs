using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    [SerializeField] private float maxHealth;
    [SerializeField] private CameraShake cameraShake;
    [SerializeField] private float cameraShakeDuration;
    [SerializeField] private float cameraShakeMagnitude;

    private float currentHealth;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TakeDamage(float amount)
    {
        currentHealth -= amount;
        
        cameraShake.StartCoroutine(cameraShake.Shake(cameraShakeDuration, cameraShakeMagnitude));

        Debug.Log("Player took " + amount + " damage.");

        if (currentHealth <= 0)
        {
            // Game over
            Debug.Log("Game over. Player died.");
        }
    }
}
