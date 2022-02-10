using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MeleeWeapon : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI text;

    // Start is called before the first frame update
    void Start()
    {
        text.gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        text.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
