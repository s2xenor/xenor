using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;
using Pathfinding;



public class Monstre : MonoBehaviour
{
    [Header("Stats")]
    [SerializeField] float speed;
    private float PlayerDetectTime;
    public float PlayerDetectRate;
    public float chaseRange;

    [Header("Attack")]
    [SerializeField] float attackRange;
    [SerializeField] int damage;
    [SerializeField] float attaclRate;
    private float lastAttackTime;

    [Header("Component")]
    Rigidbody2D rb;
    private playerwalk targetPlayer;



    [Header("PathFinding")]
    public float NextWayPointDistance = 2f;
    Path path;
    int currentPath = 0;
    bool reachEndPath = false;
    Seeker seeker;

    void Start()
    {
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();
        InvokeRepeating("UpdatePath", 0f, .5f);
    }

    void OnPathComplete(Path p)
    {
        if (!p.error)
        {
            path = p;
            currentPath = 0;
        }
    }

    void UpdatePath()
    {
        if (seeker.IsDone() && targetPlayer!=null)
        {
            seeker.StartPath(rb.position, targetPlayer.transform.position, OnPathComplete);
        }

    }

    private void FixedUpdate()
    {

        if(targetPlayer!=null)
        {
            float dist = Vector2.Distance(transform.position, targetPlayer.transform.position);
            if (dist < attackRange && Time.time - lastAttackTime >= attaclRate)
            {
                //attack
                rb.velocity = Vector2.zero;
            }
            else if (dist > attackRange)
            {
                if (path == null)
                    return;
                
                if (currentPath >= path.vectorPath.Count)
                {
                    reachEndPath = true;
                }
                else
                {
                    reachEndPath = false;
                }
                
                Vector2 direction = ((Vector2)path.vectorPath[currentPath] - rb.position).normalized;
                Vector2 force = direction * speed * Time.fixedDeltaTime;
                Debug.Log(direction);
                Debug.Log(force);

                rb.velocity = force;
                float distance = Vector2.Distance(rb.position, path.vectorPath[currentPath]);
                if (distance < NextWayPointDistance)
                {
                    currentPath++;
                }

            }
            else
            {
                rb.velocity = Vector2.zero;
            }
        }
        DetectPlayer();
    }

    void DetectPlayer()
    {
        if(Time.time-PlayerDetectTime>PlayerDetectRate)
        {
            PlayerDetectTime = Time.time;
            foreach(playerwalk player in FindObjectsOfType<playerwalk>())
            {
                if(player!=null)
                {
                    float dist = Vector2.Distance(transform.position, player.transform.position);

                    if(player == targetPlayer)
                    {
                        if(dist>chaseRange)
                        {
                            targetPlayer = null;
                        }
                    }

                    else if (dist < chaseRange)
                    {
                        if (targetPlayer == null)
                            targetPlayer = player;
                    }
                }
            }
        }
    }


}
