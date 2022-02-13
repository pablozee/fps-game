using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RocketExplosion : MonoBehaviour
{
    [SerializeField] private float explosionForce;
    [SerializeField] private float explosionRadius;
    [SerializeField] private float explosionDamage;
    [SerializeField] private GameObject rocketExplosionFX;
    [SerializeField] private List<GameObject> zombieLimbs;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        GameObject particleEffectObject = Instantiate(rocketExplosionFX, transform.position, transform.rotation);
        ParticleSystem particleSystem = particleEffectObject.GetComponent<ParticleSystem>();
        
        Collider[] collidersHit = Physics.OverlapSphere(collision.transform.position, explosionRadius);

        if (collidersHit.Length > 0)
        {
            particleSystem.Play();
        }

        foreach (Collider hitCol in collidersHit)
        {
            hitCol.gameObject.TryGetComponent<Enemy>(out Enemy enemy);
            if (enemy != null)
            {
                enemy.TakeDamage(explosionDamage);
                NavMeshAgent agent = enemy.GetComponent<NavMeshAgent>();
                agent.enabled = false;
                Rigidbody rb = enemy.GetComponent<Rigidbody>();
                rb.isKinematic = false;
                rb.useGravity = true;
                rb.AddExplosionForce(explosionForce, collision.transform.position, explosionRadius);
                Destroy(enemy.gameObject);
            }
        }

        
    }
}
