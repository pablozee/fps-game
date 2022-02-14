using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelCompleteTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        other.TryGetComponent<PlayerStats>(out PlayerStats playerStats);
        if (playerStats != null)
        {
            SceneManager.LoadScene("YouWon");
        }
    }
}
