using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private int numberOfEnemiesInLevel = 1000;
    [SerializeField] private GameObject zombiePrefab;
    [SerializeField] private int numberOfZombieMeshes;
    [SerializeField] private GameObject topRight;
    [SerializeField] private GameObject topLeft;
    [SerializeField] private GameObject bottomRight;
    [SerializeField] private GameObject bottomLeft;
    [SerializeField] private LayerMask groundLayer;

    private NavMeshTriangulation triangulation;
    private int enemyCount;


    // Start is called before the first frame update
    void Start()
    {
        Random.InitState(System.DateTime.Now.Millisecond);
        while (enemyCount < numberOfEnemiesInLevel)
        {
            OnSpawnEnemy(Random.Range(0, numberOfZombieMeshes + 1));
        }
    }

    // Update is called once per frame
    void Update()
    {
        while (enemyCount < numberOfEnemiesInLevel)
        {
            OnSpawnEnemy(Random.Range(0, numberOfZombieMeshes + 1));
        }
    }

    void OnSpawnEnemy(int spawnIndex)
    {
        float x = Random.Range(topLeft.transform.position.x, topRight.transform.position.x);
        float z = Random.Range(topLeft.transform.position.z, bottomLeft.transform.position.z);

        //  float y = -2.384186f
        float y = 0f;

        Vector3 spawnPosition = new Vector3(x, y, z);

        if (Physics.Raycast(spawnPosition, -transform.up, 2f, groundLayer))
        {
            Debug.Log("Spawning enemy");
            GameObject zombie = Instantiate(zombiePrefab, spawnPosition, Quaternion.identity, transform);
            zombie.transform.GetChild(spawnIndex + 1).gameObject.SetActive(true);
            enemyCount++;
        }
    }
}
