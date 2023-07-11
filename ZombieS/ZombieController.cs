using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ZombieController : MonoBehaviour
{
    // Start is called before the first frame update

    private NavMeshAgent agent;
    private Transform player;
    private Animator anim;
    private ZombieStats zombieStats;

    public bool isAttacking = false;
    private float attackDelayElapsed = 0;
    private float attackTimeElapsed = 0;
    private bool damageDealt = false;
    private bool canAttack = true;
    private float zombieGrowlTime;
    private float zombieGrowlElapsed = 0;
    
   
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        player = GameObject.FindWithTag("Player").transform;
        zombieStats = GetComponent<ZombieStats>();
        zombieGrowlTime = Random.Range(5, 8);
    }

    // Update is called once per frame
    void Update()
    {
            //zombieGrowlElapsed += Time.deltaTime;
            //if(zombieGrowlElapsed >= zombieGrowlTime) 
            //{
            //    zombieStats.ZombieGrowl.PlayOneShot(zombieStats.ZombieGrowl.clip);
            //    zombieGrowlTime = Random.Range(5, 8);
            //    zombieGrowlElapsed = 0;
            //}
        if (!isAttacking)
        {
            if (!canAttack)
            {
                attackDelayElapsed += Time.deltaTime;
                if (attackDelayElapsed >= zombieStats.AttackDelayTime)
                {
                    canAttack = true;
                    attackDelayElapsed = 0;
                }
            }
            agent.SetDestination(player.position);
            anim.SetFloat("Walking", agent.velocity.magnitude);
            if (Vector3.Distance(agent.transform.position, player.position) <= agent.stoppingDistance && canAttack)
            {
                isAttacking = true;
                anim.SetFloat("Walking", 0);
                anim.SetBool("isAttacking", isAttacking);
                attackTimeElapsed = 0;
            }

        }
        else
        {
            attackTimeElapsed += Time.deltaTime;
            if (attackTimeElapsed >= zombieStats.AttackAnimationTime)
            {
                attackDelayElapsed = 0;
                attackTimeElapsed = 0;
                canAttack = false;
                isAttacking = false;
                anim.SetBool("isAttacking", isAttacking);
                damageDealt = false;
            }
            if (attackTimeElapsed >= zombieStats.AttackAnimDamageTime && !damageDealt &&
                Physics.CheckSphere(transform.position + transform.forward + zombieStats.Offset, zombieStats.Range, zombieStats.PlayerLayer))
            {
                player.GetComponent<Player>().takeDamage(zombieStats.Damage);
                damageDealt = true;
            }
        }
        
    }

    public void zombieDied()
    {
        agent.isStopped = true;
        anim.SetBool("isAttacking", false);
        anim.SetFloat("Walking", 0);
        anim.SetBool("Dead", true);
		zombieStats.Audio.Play("ZombieDies");
        this.enabled = false;
    }
}
