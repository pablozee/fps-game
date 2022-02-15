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
    [SerializeField] private AudioSource explosionAudioSource;
    [SerializeField] private float cameraShakeDuration;
    [SerializeField] private float cameraShakeMagnitude;

    private Collider col;
    private Camera cam;
    private CameraShake cameraShake;

    // Start is called before the first frame update
    void Start()
    {
        col = GetComponent<Collider>();
        cam = Camera.main;
        cameraShake = cam.GetComponent<CameraShake>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        GameObject particleEffectObject = Instantiate(rocketExplosionFX, transform.position, transform.rotation);
        ParticleSystem particleSystem = particleEffectObject.GetComponent<ParticleSystem>();

        explosionAudioSource.PlayOneShot(explosionAudioSource.clip);

        StartCoroutine(cameraShake.Shake(cameraShakeDuration, cameraShakeMagnitude));

        Collider[] collidersHit = Physics.OverlapSphere(collision.transform.position, explosionRadius);

        particleSystem.Play();

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
                foreach (GameObject limb in zombieLimbs)
                {
                    GameObject spawnedLimb = Instantiate(limb, enemy.transform.position, enemy.transform.rotation);
                    Rigidbody limbRb = spawnedLimb.GetComponent<Rigidbody>();
                    limbRb.AddExplosionForce(explosionForce, collision.transform.position, explosionRadius);
                }
                Destroy(enemy.gameObject);
            }
        }


        col.enabled = false;
        Destroy(gameObject, 1f);
    }
}
