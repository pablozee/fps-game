using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DistanceOffsetTrigger : MonoBehaviour
{
    [SerializeField] private Vector3 newOffset;

    private Vector3 oldOffset;
    private Camera cam;
    private MissionWaypoint waypoint;

    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
        waypoint = cam.GetComponent<MissionWaypoint>();
        oldOffset = waypoint.offset;
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
            waypoint.offset = newOffset;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        PlayerStats playerStats;
        other.TryGetComponent<PlayerStats>(out playerStats);
        if (playerStats != null)
        {
            waypoint.offset = oldOffset;
        }
    }
}
