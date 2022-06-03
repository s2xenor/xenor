using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;
using Pathfinding;



public class Monstre : MonoBehaviour
{
    [Header("Stats")]
    [SerializeField] float speed;//speed of the monster 
    private float PlayerDetectTime;
    public float PlayerDetectRate;
    public float chaseRange;
    static public float start_pv;
    int round=1;
    private float pv = start_pv;


    [Header("Attack")]
    [SerializeField] float attackRange;
    [SerializeField] int damage;
    [SerializeField] float attaclRate;
    private float lastAttackTime;

    [Header("Component")]
    Rigidbody2D rb;//Rigidbody of the monster 
    private playerwalk targetPlayer; // component that make the player walk 
    public Animator animator;//animator of the monster 



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

    void OnCollisionEnter2D(Collision2D colision)
    {

        switch (colision.gameObject.tag)//take the tag of the object
        {
            

            case "slot (1)"://damage potion 
                /*GameObject current_player = ChangeTarget(GameObject.FindObjectsOfType<playerwalk>());
                GetDamage(current_player.GetComponent<player>().Strengt);*/
                Destroy(colision.gameObject);
                break;
        }
    }
    /*public GameObject ChangeTarget(GameObject current_player)
    {
        GameObject other_player = FindObjectsOfType<playerwalk>();
        if (other_player == current_player)
        {
            other_player = FindObjectsOfType<playerwalk>();
        }
        else if (other_player != current_player /*&& current_player.Strength < other_player.Strength)
            return other_player;
        return current_player;

    }*/

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
        animator.SetFloat("Die", 1);
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
