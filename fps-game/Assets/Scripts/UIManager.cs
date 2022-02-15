using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI levelStartText;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(RemoveLevelStartText());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator RemoveLevelStartText()
    {
        yield return new WaitForSeconds(3f);
        levelStartText.gameObject.SetActive(false);
    }
}
