using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioSource backgroundLoopSource;
    
    // Start is called before the first frame update
    void Start()
    {
        backgroundLoopSource.Play();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
