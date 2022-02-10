using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnLight : MonoBehaviour
{
    [SerializeField] private GameObject spotLightPrefab;
    private GameObject spotLight; 

    void Start()
    {
        Vector3 newPosition = transform.position;
        newPosition.x = newPosition.x - 0.08f;
        newPosition.y = newPosition.y + 6f;
        newPosition.z = newPosition.z + 1.92f;
        Vector3 newRotation = transform.rotation.eulerAngles;
        newRotation.x = 90f;
        Quaternion newRotationQuaternion = Quaternion.Euler(newRotation);
        spotLight = Instantiate(spotLightPrefab, newPosition, newRotationQuaternion, transform);

        //   if transform.rotation.eulerAngles.y = -90 then set certain x and z values..
        //   if -180 then do set different x and z values
    }

    void Update()
    {
    }
}
