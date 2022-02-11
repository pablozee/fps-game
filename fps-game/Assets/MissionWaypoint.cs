using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MissionWaypoint : MonoBehaviour
{
    public Vector3 offset;
    
    [SerializeField] private GameObject waypointUI;
    [SerializeField] private Transform target;

    private Image img;
    private GameObject background;
    private Camera cam;
    private GameObject distance;
    private TextMeshProUGUI distanceText;

    private void Start()
    {
        background = waypointUI.gameObject.transform.Find("Background").gameObject;
        img = background.GetComponent<Image>();
        cam = Camera.main;
        distance = background.gameObject.transform.Find("Distance").gameObject;
        distanceText = distance.GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        float minX = img.GetPixelAdjustedRect().width / 2;
        float maxX = Screen.width - minX;

        float minY = img.GetPixelAdjustedRect().height / 2;
        float maxY = Screen.height - minY;

        Vector2 pos = cam.WorldToScreenPoint(target.position + offset);

        if (Vector3.Dot((target.position - transform.position), transform.forward) < 0)
        {
            if (pos.x < Screen.width / 2)
            {
                pos.x = maxX;
            }
            else
            {
                pos.x = minX;
            }
        }

        pos.x = Mathf.Clamp(pos.x, minX, maxX);
        pos.y = Mathf.Clamp(pos.y, minY, maxY);

        img.transform.position = pos;
        distanceText.text = ((int)Vector3.Distance(target.position, transform.position)).ToString() + "m";
    }
}
