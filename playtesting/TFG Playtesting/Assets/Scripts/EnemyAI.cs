using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{

    public NavMeshAgent agent;
    public Transform player;
    public Transform projectileSpawn;
    public LayerMask whatIsPlayer, whatIsGround;
    

    //Patrol
    private Vector3 walkpoint;
    private bool walkPointSet;
    [Header("States")]
    public float walkPointRange;
    public float sightRange;
    public float attackRange;
    public float timeToStart = 4f;
    private float timeToStartTimer;
    private bool playerInSightRange;
    private bool playerInAttackRange;


    
    //Attack
    public enum EnemyType {Enemy1Var1, Enemy1Var2, Enemy1Var3, Enemy2Var1, Enemy2Var2, Enemy2Var3, Enemy2Var4, Enemy2Var5};
    [Header("Attack")]
    public EnemyType enemyType;
    public bool shotgunAttack = true;
    public float projectileSpeed;
    public float timeBetweenAttacks;
    public float timeBetweenBullets;
    public int amountOfShotgunBullets;
    public float projectileSpread = 90f;

    private bool alreadyAttacked;
    public GameObject projectile;
    public GameObject shotgunProjectile;

    
    private void Awake()
    {
        player = GameObject.Find("PlayerObj").transform;
        agent = GetComponent<NavMeshAgent>();
        timeToStartTimer = timeToStart;
    }
    // Start is called before the first frame update

    // Update is called once per frame
    private void Update()
    {
        if (timeToStartTimer > 0) timeToStartTimer -= Time.deltaTime;
        else
        {
            playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
            playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);

            if (!playerInSightRange && !playerInAttackRange) Patroling();
            if (playerInSightRange && !playerInAttackRange) Chasing();
            if (playerInSightRange && playerInAttackRange)
            {
                if (shotgunAttack) ShotgunAttack();
                else BasicAttack();
            } 
        }
       
    }

    private void Patroling()
    {
        if (!walkPointSet) SearchWalkPoint();

        if (walkPointSet)
        {
            agent.SetDestination(walkpoint);
        }

        Vector3 distanceToWalkPoint = transform.position - walkpoint;

        if (distanceToWalkPoint.magnitude < 1)
        {
            walkPointSet = false;
        }
    }

    private void SearchWalkPoint()
    {
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);

        walkpoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        if (Physics.Raycast(walkpoint, -transform.up, 2f, whatIsGround))walkPointSet = true;
    }

    private void Chasing()
    {
        agent.SetDestination(player.position);
    }

    private void BasicAttack()
    {
        agent.SetDestination(transform.position);

        transform.LookAt(player);

        if (!alreadyAttacked)
        {
            FireBullet();         
            Invoke(nameof(FireBullet), timeBetweenBullets);   

            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
    }

    private void ShotgunAttack()
    {
        agent.SetDestination(transform.position);

        transform.LookAt(player);

        if (!alreadyAttacked)
        {
            FireShotgun();           

            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
    }

    private void ResetAttack()
    {
        alreadyAttacked = false;
    }

    private void FireBullet()
    {
        Rigidbody rb = Instantiate(projectile, projectileSpawn.position, Quaternion.identity).GetComponent<Rigidbody>();
        rb.AddForce(transform.forward.normalized * projectileSpeed, ForceMode.Impulse);
    }

    private void FireShotgun()
    {
        float startRotation = -projectileSpread * 0.5f;
        float angleIncrease = projectileSpread / ((float)amountOfShotgunBullets - 1f);
        transform.RotateAround(transform.position, Vector3.up, startRotation);

        for (int i = 0; i < amountOfShotgunBullets; i++)
        {
            //float tempRot = startRotation + angleIncrease * i;
        
            Rigidbody rb = Instantiate(projectile, projectileSpawn.position, Quaternion.identity).GetComponent<Rigidbody>();
            rb.AddForce(transform.forward.normalized * projectileSpeed, ForceMode.Impulse);
            transform.RotateAround(transform.position, Vector3.up, angleIncrease);
            
        }
        
    }

    private void OnCollisionEnter(Collision col)
    {

        if (col.gameObject.tag == "Death")
        {
            Destroy(this.gameObject);
        }
        
    }

}
