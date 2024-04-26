using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    private NavMeshAgent agent;
    public Transform[] patrolPoints;

    private bool isPatrolling = true;
    private float waitTime;
    private Transform player;
    public float detectionRadius = 10f;
    private float attackRadius = 5f;
    private float chaseSpeed = 3f;
    private float chaseTimer = 0f;
    private bool inCooldown = false;
    public float cooldownDuration = 1f;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        SelectNextPatrolPoint();
    }

    private void Update()
    {
        if (isPatrolling){
           // Debug.Log("patrullando");
            PatrolBehavior();}
        else{
           // Debug.Log("nelpastel");
            ChaseBehavior();}
    }

    private void PatrolBehavior()
    {
        waitTime -= Time.deltaTime;
        if (waitTime <= 0)
        {//Debug.Log('1');
            SelectNextPatrolPoint();
        }

        if (Vector3.Distance(transform.position, player.position) < detectionRadius)
        {//Debug.Log('2');
            isPatrolling = false;
        }
    }

    private void ChaseBehavior()
    {   
        if (!inCooldown)
        {agent.speed = chaseSpeed;
            if (Vector3.Distance(transform.position, player.position) < detectionRadius)//> attackRadius)
            {
                agent.SetDestination(player.position);
                
                chaseTimer += Time.deltaTime;
                
                if (chaseTimer >= 5f)
                {
                    inCooldown = true;
                    chaseTimer = 0f;
                }
            }
            else
            {
                //agent.speed = 0;
                isPatrolling = true;
                SelectNextPatrolPoint();
            }
        }
        else
        {
            // In cooldown, stop chasing for the cooldown duration
            agent.speed = 0;
            chaseTimer += Time.deltaTime;
            if (chaseTimer >= cooldownDuration)
            {
                inCooldown = false;
                chaseTimer = 0f;
            }
        }
    }

    private void SelectNextPatrolPoint()
    {
        waitTime = Random.Range(0.5f, 3f);
        int nextPoint = Random.Range(0, patrolPoints.Length);
        agent.SetDestination(patrolPoints[nextPoint].position);
    }
}








// using UnityEngine;
// using UnityEngine.AI;
// using Unity.AI.Navigation;


// public class EnemyController : MonoBehaviour
// {
//     public NavMeshAgent agent;
//     public NavMeshPathFollower pathFollower;
//     public NavMeshPursuitMovement pursuitMovement;

//     private float waitTime;
//     private bool isPatrolling = true;

//     private void Start()
//     {
//         agent = GetComponent<NavMeshAgent>();
//         pathFollower = GetComponent<NavMeshPathFollower>();
//         pursuitMovement = GetComponent<NavMeshPursuitMovement>();

//         pathFollower.enabled = true;
//         pursuitMovement.enabled = false;

//         SelectNextPatrolPoint();
//     }

//     private void Update()
//     {
//         if (pursuitMovement.isDetected)
//         {
//             pathFollower.enabled = false;
//             pursuitMovement.enabled = true;
//             isPatrolling = false;
//         }
//         else if (!isPatrolling)
//         {
//             waitTime -= Time.deltaTime;
//             if (waitTime <= 0)
//             {
//                 isPatrolling = true;
//                 pathFollower.enabled = true;
//                 pursuitMovement.enabled = false;
//                 SelectNextPatrolPoint();
//             }
//         }
//     }

//     private void SelectNextPatrolPoint()
//     {
//         waitTime = Random.Range(6f, 10f);
//         int nextPoint = Random.Range(0, pathFollower.path.corners.Length);
//         agent.SetDestination(pathFollower.path.corners[nextPoint]);
//     }
// }