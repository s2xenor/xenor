using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;

public class Monsters : MonoBehaviour
{

    void Start()
    {
        
    }



    // Pathfinding : https://github.com/h8man/NavMeshPlus
    // Attach NavMeshAgent component to monster
    // Read the wiki of the github

    
    Transform target;       // Target of Pathfinding
    NavMeshAgent agent;     // Pathfinding
    float[] distanceFactor;   // Facteur de distance entre les joueurs < 1 (ex : 1/2 -> change de target si l'autre joueur et a la moitié de la distance de l'autre))
    GameObject[] playersObject;
    Transform[] playersTransform;
    public Animator animator;
    int round;

    // Stats of monster
    public float attack;
    float pv;
    float start_pv;

    public Monsters(float speed, float attack, float pv, float stoppingDistance, float distanceFactor)
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        agent.speed = speed;
        agent.stoppingDistance = stoppingDistance;
        playersObject = GameObject.FindGameObjectsWithTag("Player");
        playersTransform = new Transform[playersObject.Length];
        this.distanceFactor = new float[2] { distanceFactor, 1 / distanceFactor };
        this.attack = attack;
        this.pv = pv;
        start_pv = pv;
        round = 1;

        for (int i = 0; i < playersObject.Length; i++)
            playersTransform[i] = playersObject[i].GetComponent<Transform>();
    }

    // Make AI things
    public void AI() // /!\ Call in FixedUpdate()
    {
        CheckDistance();    // Check if should change target

        if (!agent.isStopped)
        {
            agent.SetDestination(target.position);  // Move GameObject
            animator.SetFloat("Moving", (0.5f));
            // Moving animation
        }
        else
        {
            Attack();
        }
    }

    public void ChangeTarget(Transform transform)
    {
        target = transform;
    }

    public void ChangeTarget(GameObject go)
    {
        target = go.GetComponent<Transform>();
    }

    public void ChangeSpeed(float speed)
    {
        agent.speed = speed;
    }

    // Check if should change target if one player is closer than the other
    void CheckDistance()
    {
        Transform tmp = target;
        float[] distance = new float[2];

        // Get distance to players
        for (int i = 0; i < playersTransform.Length; i++)
        {
            target = playersTransform[i];
            distance[i] = agent.remainingDistance;
        }

        float factor = 1;

        // Get distance factor
        if (distance[1] != 0)
            factor = distance[0] / distance[1];

        // Check if should change target according to players position and given distance factor
        if (factor < distanceFactor[0])
            target = playersTransform[0];
        else if (factor > 1 / distanceFactor[1])
            target = playersTransform[1];
        else
            target = tmp;
    }

    public void GetDamage(float damage)
    {
        pv -= damage;

        if (pv <= 0)
            Die();
        else
        {
            // Run damaged animation
            animator.SetFloat("Damage", (0.5f));
        }
    }

    void Die()
    {
        // Run death animation
        animator.SetFloat("Die",1);
    }

    void Attack()
    {
        // Run Attack animation
        if (round % 2 == 0)
        { 
            animator.SetFloat("Attack", (0.5f)); //Animation attack number 1
        }
        else
        {
            animator.SetFloat("Attack", (0.1f));//Animation attack number 2
        }
    }

    public void Heal()
    {
        pv = start_pv;
    }
}
