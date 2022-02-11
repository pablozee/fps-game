using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DistanceOffsetTrigger : MonoBehaviour
{
    private Camera cam;
    private MissionWaypoint waypoint;

    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
        waypoint = cam.GetComponent<MissionWaypoint>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        PlayerStats playerStats;
        other.TryGetComponent<PlayerStats>(out playerStats);
        if (playerStats != null)
        {
            waypoint.offset = new Vector3(0f, 0f, 0f);
        }
    }
}
