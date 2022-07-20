using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum EnemyState
{
    Idle,
    Patrol,
    Chase,
    Attacking
}

public class EnemyBehaviour : MonoBehaviour
{
    public Transform playerPos;
    public Collider weapon;

    [SerializeField] float viewAngle = 140.0f;
    [SerializeField] float viewDistance = 10.0f;
    [SerializeField] float movementSpeed = 5.0f;
    [SerializeField] float attackDistance = 1.5f;
    [SerializeField] float health = 100.0f;
    [SerializeField] float damage = 15.0f;
    [SerializeField] PlayerController player;
    
    Rigidbody rb;
    EnemyState cState;
    bool attacking = false;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        ChangeState(EnemyState.Idle);
    }

    // Update is called once per frame
    void Update()
    {
        if (health > 0)
        {
            Debug.Log(cState);

            switch (cState)
            {
                case EnemyState.Idle:
                IdleState();
                break;

                case EnemyState.Patrol:
                viewAngle = 360.0f;
                IdleState();
                break;

                case EnemyState.Chase:
                ChaseState();
                break;

                case EnemyState.Attacking:
                AttackState();
                break;
            }
        }
        else
        {
            Die();
        }
    }
    
    bool FindPlayer()
    {
        if (Vector3.Distance(transform.position, playerPos.position) < viewDistance)
        {
            Vector3 directionToPlayer = (playerPos.position - transform.position).normalized;
            float angleBetweenGuardAndPlayer = Vector3.Angle(transform.forward, directionToPlayer);
            if (angleBetweenGuardAndPlayer < viewAngle / 2)
            {
                Debug.Log("viewAngle " + viewAngle);
                Debug.DrawLine(transform.position, playerPos.position, Color.red, 0.1f);
                
                if (!Physics.Linecast(transform.position, playerPos.position))
                {
                    return true;
                }
            }
        }
        return false;
    }

    void IdleState()
    {
        if (FindPlayer()) ChangeState(EnemyState.Chase);
    }

    void ChaseState()
    {
        if (Vector3.Distance(transform.position, playerPos.position) > attackDistance)
        {
            transform.LookAt(playerPos);
            gameObject.GetComponent<Animator>().Play("Locomotion Normal", 0);
            Debug.DrawLine(transform.position, playerPos.position, Color.red, 0.1f);
        }
        else if (Vector3.Distance(transform.position, playerPos.position) > viewDistance)
        {
            ChangeState(EnemyState.Patrol);
        }
        else
        {
            ChangeState(EnemyState.Attacking);
        }
    }

    void AttackState()
    {
        if (Vector3.Distance(transform.position, playerPos.position) > attackDistance)
        {
            ChangeState(EnemyState.Chase);
            attacking = false;
        }
        else
        {
            attacking = true;
            gameObject.GetComponent<Animator>().Play("Attack", 0);
        }
    }

    void OnTriggerEnter(Collider col)
    {
        col = weapon;

        if (attacking)
        {
            player.TakeDamage(Random.Range(damage * 0.5f, damage * 1.5f));
        }
    }

    public void TakeDamage(float dmg)
    {
        health -= dmg;
    }

    void Die()
    {
        rb.isKinematic = false;
        GetComponent<Animator>().enabled = false;
    }

    void ChangeState(EnemyState state) => cState = state;
}
