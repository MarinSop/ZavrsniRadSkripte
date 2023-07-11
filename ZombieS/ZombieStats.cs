using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ZombieStats : MonoBehaviour
{
    [SerializeField]
    private float health;
    [SerializeField]
    private float damage;
    [SerializeField]
    private int points;
    [SerializeField]
    private float range;
    [SerializeField]
    private Vector3 offset;
    [SerializeField]
    private LayerMask playerLayer;
    [SerializeField]
    private ParticleSystem blood;
    [SerializeField]
    private float stunSpeed;

    [SerializeField]
    private float attackDelayTime;
    [SerializeField]
    private float attackAnimationTime;
    [SerializeField]
    private float attackAnimDamageTime;

    [SerializeField]
    private float pickupDropChance;
    [SerializeField]
    private GameObject ammoPickup;
    [SerializeField]
    private GameObject healthPickup;

    private bool dead = false;

    private NavMeshAgent agent;
	private AudioManager audio;
    private AudioSource zombieGrowl;

	public float Health { get => health; set => health = value; }
    public float Damage { get => damage; set => damage = value; }
	public int Points { get => points; set => points = value; }
	public float Range { get => range; set => range = value; }
    public Vector3 Offset { get => offset; set => offset = value; }
    public LayerMask PlayerLayer { get => playerLayer; set => playerLayer = value; }
    public float AttackDelayTime { get => attackDelayTime; set=> attackDelayTime = value; }
    public float AttackAnimationTime { get => attackAnimationTime; set => attackAnimationTime = value; }
    public float AttackAnimDamageTime { get => attackAnimDamageTime; set => attackAnimDamageTime = value; }
	public AudioManager Audio { get => audio; }
	public AudioSource ZombieGrowl { get => zombieGrowl; }

	public bool Dead { get => dead; set => dead = value; }

    void Start()
    {
        zombieGrowl = GetComponent<AudioSource>();
        agent = GetComponent<NavMeshAgent>();
        audio = FindObjectOfType<AudioManager>();
    }

    void Update()
    {
        if (health <= 0 && !dead)
        {
            dead = true;
            Player player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
            player.Points += points;
            player.updatePointsUI();
            GetComponent<ZombieController>().zombieDied();
            GetComponent<Collider>().enabled = false;
            GameObject.FindGameObjectWithTag("GameManager").GetComponent<ZombiesSpawnManager>().zombieDied();
            if(Random.Range(0.0f,100.0f) <= pickupDropChance)
            {
                if(Random.Range(0.0f,100.0f) <= 50.0f)
                {
                    Instantiate(ammoPickup,transform.position, Quaternion.identity);
                }
                else
                {
					Instantiate(healthPickup, transform.position, Quaternion.identity);
				}
            }
            Destroy(gameObject, 2.0f);
        }
    }

    public void takeDamage(float damage)
    {
        health -= damage;
        blood.Play();
        audio.Play("ZombieHurt");
        agent.velocity = (agent.velocity.magnitude * stunSpeed) * agent.velocity.normalized;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position + transform.forward + offset, range);
    }
}
