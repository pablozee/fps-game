using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DisableOutOfView : MonoBehaviour
{
    
    private GameObject player;
    private Camera cam;
    private Enemy enemy;
    private NavMeshAgent agent;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("First Person Player");
        cam = Camera.main;
        enemy = GetComponent<Enemy>();
        agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 viewportPosition = cam.WorldToViewportPoint(transform.position);

        if (viewportPosition.x > 0 && viewportPosition.x < 1)
        {
            if (viewportPosition.y > 0 && viewportPosition.y < 1)
            {
                if (viewportPosition.z > 0)
                {
                    enemy.enabled = true;
                    agent.enabled = true;
                }
            }
        } 
        else
        {
            enemy.enabled = false;
            agent.enabled = false;
        }

        if (Vector3.Distance(transform.position, player.transform.position) < 20)
        {
            enemy.enabled = true;
            agent.enabled = true;
        }
    }
}
