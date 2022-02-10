using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MeleeWeapon : MonoBehaviour
{
    [SerializeField] private GameObject ammoDisplay;

    // Start is called before the first frame update
    void Start()
    {
        ammoDisplay.SetActive(false);
    }

    private void OnEnable()
    {
        ammoDisplay.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
