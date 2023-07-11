using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class PlayerController : MonoBehaviour
{
    private Rigidbody rb;
    private Player playerStats;
    [SerializeField]
    private Animator animator;
    private Vector3 direction;
	private Vector3 shootDirection;

	public GameObject[] primaryWeapons;

	public GameObject[] secondaryWeapons;

    [SerializeField]
    private LayerMask enemyMask;

    [SerializeField]
    private LayerMask floorMask;


    private bool isMeleeAttack = false;
    private bool canAttack = true;
    [SerializeField]
    private float meleeAttackTime;
    [SerializeField]
    private float meleeAttackDamageTime;
    private float meleeAttackTimeElapsed = 0;

	// Start is called before the first frame update
	void Start()
    {
        rb = GetComponent<Rigidbody>();
        playerStats = GetComponent<Player>();
    }

    private void Update()
    {
		direction = new Vector3(SimpleInput.GetAxis("Horizontal"), 0f, SimpleInput.GetAxis("Vertical")).normalized;
        shootDirection = new Vector3(SimpleInput.GetAxis("LookHorizontal"), 0f, SimpleInput.GetAxis("LookVertical"));
		switchWeapons();
        meleeAttack();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
		Vector3 velocity = direction * playerStats.Speed * Time.deltaTime;
        rb.velocity = velocity;
		Quaternion vecRotation = Quaternion.FromToRotation(rb.velocity.normalized, transform.forward);
		float velAngle = vecRotation.eulerAngles.y;
		//if (Input.GetButton("Fire1"))
        if(shootDirection.magnitude >= 0.1)
		{
            animationHandler(velAngle);
			//Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			//if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity,floorMask))
			//{
			//Vector3 lookDirection = hit.point - transform.position;
			//float rotation = Mathf.Atan2(lookDirection.x, lookDirection.z) * Mathf.Rad2Deg;
			float rotation = Mathf.Atan2(shootDirection.x, shootDirection.z) * Mathf.Rad2Deg;
			transform.rotation = Quaternion.Euler(0, rotation, 0);
			//}
		}
		else if (rb.velocity.magnitude > 0.1)
		{
            if (!isMeleeAttack)
            {
                playerStats.Audio.CheckPlay("Footsteps");
                animator.SetFloat("Running", 1);
                float rotation = Mathf.Atan2(rb.velocity.x, rb.velocity.z) * Mathf.Rad2Deg;
                transform.rotation = Quaternion.Euler(0, rotation, 0);
            }
            else
            {
				animationHandler(velAngle);
			}
		}
		else
		{
			playerStats.Audio.Stop("Footsteps");
			animator.SetFloat("Running", 0);
			animator.SetFloat("Strafe", 0);
		}
	}


    void animationHandler(float angle)
    {
		playerStats.Audio.CheckPlay("Footsteps");
		if (rb.velocity.magnitude > 0.1)
		{
			if (angle < 45 || angle > 315)
			{
				animator.SetFloat("Running", 1);
				animator.SetFloat("Strafe", 0);
			}
			else if (angle > 45 && angle < 135)
			{
				animator.SetFloat("Running", 0);
				animator.SetFloat("Strafe", -1);
			}
			else if (angle > 135 && angle < 225)
			{
				animator.SetFloat("Running", -1);
				animator.SetFloat("Strafe", 0);
			}
			else if (angle > 225 && angle < 315)
			{
				animator.SetFloat("Running", 0);
				animator.SetFloat("Strafe", 1);
			}
		}
		else
		{
			playerStats.Audio.Stop("Footsteps");
			animator.SetFloat("Running", 0);
			animator.SetFloat("Strafe", 0);
		}
	}

    void meleeAttack()
    {
		//if(playerStats.ActiveWeapon == WeaponType.Melee && Input.GetButton("Fire1") && !isMeleeAttack && !playerStats.CantAttack)
		if (playerStats.ActiveWeapon == WeaponType.Melee && shootDirection.magnitude >= 0.1 && !isMeleeAttack && !playerStats.CantAttack)
		{
            isMeleeAttack = true;
            meleeAttackTimeElapsed = 0;
            animator.SetBool("MeleeAttack", true);

        }

        if(isMeleeAttack)
        {
            meleeAttackTimeElapsed += Time.deltaTime;
            if(meleeAttackTimeElapsed >= meleeAttackDamageTime && canAttack)
            {
				Collider[] enemies = Physics.OverlapSphere(transform.position + transform.forward + playerStats.MeleeAttackPosition, playerStats.MeleeAttackRange, enemyMask);
				foreach (var enemy in enemies)
				{
					enemy.GetComponent<ZombieStats>().takeDamage(50);
				}
                canAttack = false;
			}
            else if(meleeAttackTimeElapsed >= meleeAttackTime)
            {
                canAttack = true;
				animator.SetBool("MeleeAttack", false);
                isMeleeAttack = false;
			}
        }

    }


    void switchWeapons()
    {
        int primaryChildCount = playerStats.PrimaryHolder.childCount;
        int secondaryChildCount = playerStats.SecondaryHolder.childCount;
        if (SimpleInput.GetKeyDown(KeyCode.Alpha1) && primaryChildCount > 0)
        {
            playerStats.ActiveWeapon = WeaponType.Primary;
            playerStats.PrimaryHolder.GetChild(0).gameObject.SetActive(true);
            if(secondaryChildCount > 0)
                playerStats.SecondaryHolder.GetChild(0).gameObject.SetActive(false);
            playerStats.Melee.gameObject.SetActive(false);
            playerStats.updateAmmoUI();
            animator.SetInteger("WeaponType", 1);
        }
        else if (SimpleInput.GetKeyDown(KeyCode.Alpha2) && secondaryChildCount > 0)
        {
            playerStats.ActiveWeapon = WeaponType.Secondary;
            playerStats.SecondaryHolder.GetChild(0).gameObject.SetActive(true);
            if (primaryChildCount > 0)
                playerStats.PrimaryHolder.GetChild(0).gameObject.SetActive(false);
            playerStats.Melee.gameObject.SetActive(false);
            playerStats.updateAmmoUI();
            animator.SetInteger("WeaponType", 2);
        }
        else if (SimpleInput.GetKeyDown(KeyCode.Alpha3))
        {
            playerStats.ActiveWeapon = WeaponType.Melee;
            if (primaryChildCount > 0)
                playerStats.PrimaryHolder.GetChild(0).gameObject.SetActive(false);
            if (secondaryChildCount > 0)
                playerStats.SecondaryHolder.GetChild(0).gameObject.SetActive(false);
            playerStats.Melee.gameObject.SetActive(true);
            playerStats.updateAmmoUI();
            animator.SetInteger("WeaponType", 0);
        }

    }

    public void getPrimary(int index)
    {
        GunStats stats = primaryWeapons[index].GetComponent<GunStats>();

        if (stats.Cost <= playerStats.Points)
        {
            playerStats.buy(stats.Cost);
            if (playerStats.PrimaryHolder.childCount > 0)
                Destroy(playerStats.PrimaryHolder.GetChild(0).gameObject);
            GameObject gunObj = Instantiate(primaryWeapons[index], playerStats.PrimaryHolder.position, playerStats.PrimaryHolder.rotation);
            gunObj.transform.SetParent(playerStats.PrimaryHolder);
            gunObj.SetActive(false);
		}
    }

    public void getSecondary(int index)
    {
        GunStats stats = secondaryWeapons[index].GetComponent<GunStats>();

        if (stats.Cost <= playerStats.Points)
        {
			playerStats.buy(stats.Cost);
			if (playerStats.SecondaryHolder.childCount > 0)
                Destroy(playerStats.SecondaryHolder.GetChild(0).gameObject);
            GameObject gunObj = Instantiate(secondaryWeapons[index], playerStats.SecondaryHolder.position, playerStats.SecondaryHolder.rotation);
            gunObj.transform.SetParent(playerStats.SecondaryHolder);
            gunObj.SetActive(false);
        }

    }

    public void dead()
    {
        animator.SetFloat("Running", 0);
        animator.SetFloat("Strafe", 0);
        animator.SetInteger("WeaponType", 0);
        animator.SetBool("MeleeAttack", false);
        animator.SetBool("isDead", true);
        rb.velocity = Vector3.zero;
        this.enabled = false;
        FindObjectOfType<PauseMenu>().loadScene(SceneManager.GetActiveScene().name);
    }

}
