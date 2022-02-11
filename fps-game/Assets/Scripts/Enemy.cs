using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private LayerMask whatIsGround, whatIsPlayer;
    private Transform player;

    // Patroling
    [SerializeField] Vector3 walkPoint;
    private bool walkPointSet = false;
    [SerializeField] private float walkPointRange;

    // Attacking
    [SerializeField] private float timeBetweenAttacks;
    private bool alreadyAttacked;

    //States
    [SerializeField] private float sightRange, attackRange;
    [SerializeField] private bool playerInSightRange, playerInAttackRange;

    private Animator anim;
    [SerializeField] private float maxHealth = 100f;
    private float currentHealth;
    [SerializeField] private Transform attackPoint;
    [SerializeField] private float attackRadius = 0.5f;
    [SerializeField] private float attackDamage = 5f;

    private Collider collider;
    private bool isAlive;

    private NavMeshTriangulation triangulation;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponentInChildren<Animator>();
        collider = GetComponent<CapsuleCollider>();
        player = GameObject.Find("First Person Player").transform;
    }

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        isAlive = true;
        triangulation = NavMesh.CalculateTriangulation();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isAlive) return;

        /*
        if (Vector3.Distance(player.transform.position, transform.position) > 20)
        {
            agent.enabled = false;
            return;
        }
        else
        {
            agent.enabled = true;
        }
        */

        MovementAnimations();

        // Check for sight and attack range
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);

        if (!playerInSightRange && !playerInAttackRange) Patroling();
        if (playerInSightRange && !playerInAttackRange) ChasePlayer();
        if (playerInAttackRange && playerInSightRange) AttackPlayer();

        Vector3 newRotation = transform.localEulerAngles;
        newRotation.x = 0f;
        transform.localEulerAngles = newRotation;

        
    }

    private void Patroling()
    {
        if (!walkPointSet) SearchWalkPoint();

        if (walkPointSet)
            agent.SetDestination(walkPoint);

        Vector3 distanceToWalkPoint = transform.position - walkPoint;

        // Walkpoint reached
        if (distanceToWalkPoint.magnitude < 1f)
            walkPointSet = false;
    }

    private void SearchWalkPoint()
    {
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);

        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);
        
        /*
        int vertexIndex = Random.Range(0, triangulation.vertices.Length);

        NavMeshHit hit;
        if (NavMesh.SamplePosition(triangulation.vertices[vertexIndex], out hit, 2f, 0))
        {
            walkPoint = triangulation.vertices[vertexIndex];
            walkPointSet = true;
        }

        */
        if (Physics.Raycast(walkPoint, -transform.up, 2f, whatIsGround))
        {
            walkPointSet = true;
        }
    }

    private void ChasePlayer()
    {
        agent.SetDestination(player.position);
    }

    private void AttackPlayer()
    {
        // Make sure enemy doesn't move
        agent.SetDestination(transform.position);

        transform.LookAt(player);

        if (!alreadyAttacked)
        {
            // Play attack animation
            SetAttackTrigger();

            CheckAttackRadius();

            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
    }

    private void CheckAttackRadius()
    {
        Collider[] hitPlayers = Physics.OverlapSphere(attackPoint.position, attackRange, whatIsPlayer);

        foreach (Collider player in hitPlayers)
        {
            Debug.Log("Hit " + player.name);

            PlayerStats playerStats = player.gameObject.GetComponent<PlayerStats>();

            playerStats.TakeDamage(attackDamage);
        }
    }

    private void ResetAttack()
    {
        alreadyAttacked = false;
    }

    public void TakeDamage(float amount)
    {
        if (!isAlive) return;

        // Play take damage animation
        SetTakeDamageTrigger();


        currentHealth -= amount;
        if (currentHealth <= 0f)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log(transform.name + " died");
        SetDieTrigger();
        agent.enabled = false;
        isAlive = false;    
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);
    }

    void MovementAnimations()
    {
        anim.SetFloat("MoveMagnitude", agent.velocity.magnitude);
    }

    void SetAttackTrigger()
    {
        anim.SetTrigger("Attack");
    }

    void SetDieTrigger()
    {
        anim.SetTrigger("Die");
    }

    void SetTakeDamageTrigger()
    {
        anim.SetTrigger("TakeDamage");
    }
}
