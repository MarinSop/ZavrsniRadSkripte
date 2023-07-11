using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Player : MonoBehaviour
{
    [SerializeField]
    private float maxHealth;
    private float health;
    [SerializeField]
    private float speed;
    [SerializeField]
    private int points;


    private TextMeshProUGUI ammoUI;
    private TextMeshProUGUI pointsUI;

    private Slider healthUI;
    [SerializeField]
    private Transform primaryHolder;
    [SerializeField]
    private Transform secondaryHolder;
    [SerializeField]
    private Transform melee;
    [SerializeField]
    private Vector3 meleeAttackPosition;
    [SerializeField]
    private float meleeAttackRange;
    [SerializeField]
    private ParticleSystem blood;
    private WeaponType activeWeapon = WeaponType.Melee;
    private AudioManager audio;
    private bool cantAttack = false;


    public float Health { get => health; set => health = value; }
	public float MaxHealth { get => maxHealth; set => maxHealth = value; }
	public float Speed { get => speed; set => speed = value; }
	public int Points { get => points; set => points = value; }
	public Transform PrimaryHolder { get => primaryHolder; set => primaryHolder = value; }
    public Transform SecondaryHolder { get => secondaryHolder; set => secondaryHolder = value; }
    public Transform Melee { get => melee; set => Melee = value; }
	public Vector3 MeleeAttackPosition { get => meleeAttackPosition; set => meleeAttackPosition = value; }
	public float MeleeAttackRange { get => meleeAttackRange; set => meleeAttackRange = value; }
	internal WeaponType ActiveWeapon { get => activeWeapon; set => activeWeapon = value; }
    public TextMeshProUGUI AmmoUI { get => ammoUI; set => ammoUI = value; }
	public TextMeshProUGUI PointsUI { get => pointsUI; set => pointsUI = value; }
    public bool CantAttack { get => cantAttack; set => cantAttack = value; }
	public AudioManager Audio { get => audio;}

	private void OnTriggerEnter(Collider other)
	{
		if(other.CompareTag("Pickup"))
        {
            other.GetComponent<Pickup>().pickingUp(this);
        }
        if(other.CompareTag("Shop"))
        {
            other.GetComponent<Shop>().setPlayerClose(true);
        }
        if(other.CompareTag("PurchasableBarrier"))
        {
            other.GetComponent<PurchaseBarrier>().DeOrActiveCostInfo();
        }
        else if(other.CompareTag("End"))
		{
			other.GetComponent<PurchaseBarrier>().DeOrActiveCostInfo();
		}
	}

	private void OnTriggerExit(Collider other)
	{
		if (other.CompareTag("Shop"))
		{
			other.GetComponent<Shop>().setPlayerClose(false);
		}
		if (other.CompareTag("PurchasableBarrier"))
		{
			other.GetComponent<PurchaseBarrier>().DeOrActiveCostInfo();
		}
		else if (other.CompareTag("End"))
		{
			other.GetComponent<PurchaseBarrier>().DeOrActiveCostInfo();
		}
	}

	private void OnTriggerStay(Collider other)
	{
        if (SimpleInput.GetButtonDown("Interact"))
        {
            if (other.CompareTag("PurchasableBarrier"))
            {
                other.GetComponent<PurchaseBarrier>().purchase(this);
            }
            else if (other.CompareTag("End"))
            {
                other.GetComponent<PurchaseBarrier>().buyEnd(this);
            }
        }
	}



	void Start()
    {
        healthUI = GameObject.FindGameObjectWithTag("healthUI").GetComponent<Slider>();
		pointsUI = GameObject.FindGameObjectWithTag("pointsUI").GetComponent<TextMeshProUGUI>();
		ammoUI = GameObject.FindGameObjectWithTag("ammoUI").GetComponent<TextMeshProUGUI>();
        health = maxHealth;
        updatePointsUI();
        updateAmmoUI();
        audio = FindObjectOfType<AudioManager>();
	}

    public void updateAmmoUI() 
    {
        GunStats gunStats = null;
        switch(activeWeapon)
        {
            case WeaponType.Primary:
                if(primaryHolder.childCount > 0)
                {
                    gunStats = primaryHolder.GetChild(0).GetComponent<GunStats>();
                }
                break;
            case WeaponType.Secondary:
                if (secondaryHolder.childCount > 0)
                {
                    gunStats = secondaryHolder.GetChild(0).GetComponent<GunStats>();
                }
                break;
            case WeaponType.Melee:
                gunStats = null;
                break;
            default:
                break;
        }
        if(gunStats != null)
        {
            AmmoUI.text = gunStats.CurrentClipAmmo + "/" + gunStats.CurrentAmmo;
        }
        else
        {
            AmmoUI.text = "0/0";
        }

    }

    public void updatePointsUI()
    {
        pointsUI.text = "POINTS: " + points;
	}

    public void takeDamage(float damage)
    {
        health -= damage;
        blood.Play();
		healthUI.value = health / maxHealth;
        audio.Play("PlayerHurt");
        if(health <= 0)
        {
            GetComponent<PlayerController>().dead();
        }
    }

    public void restoreHealth()
    {
        health = maxHealth;
		healthUI.value = health / maxHealth;
	}

    public void buy(int cost)
    {
        points -= cost;
        audio.Play("Buy");
        updatePointsUI();
    }

}
