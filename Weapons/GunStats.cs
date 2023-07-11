using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum WeaponType
{
    Primary, Secondary, Melee
}

public class GunStats : MonoBehaviour
{
    [SerializeField]
    private string gunName;
    [SerializeField]
    private string shotSoundName;
    [SerializeField]
    private WeaponType type;
	[SerializeField]
	private int cost;
	[SerializeField]
    private Transform shootingPoint;
    [SerializeField]
    private GameObject bulletTracer;
    [SerializeField]
    private float range;
    [SerializeField]
    private float bulletSpeed;
    [SerializeField]
    private float accuracy;
    [SerializeField]
    private float fireRate;
    [SerializeField]
    private float damage;

    private int currentAmmo;
    private int currentClipAmmo;
    [SerializeField]
    private int maxClip;
    [SerializeField]
    private float reloadTime;
    [SerializeField]
    private int maxAmmo;

    public string GunName { get => gunName; set => gunName = value; }
	public string ShotSoundName { get => shotSoundName; set => shotSoundName = value; }
	public Transform ShootingPoint { get => shootingPoint; set => shootingPoint = value; }
	public GameObject BulletTracer { get => bulletTracer; set => bulletTracer = value; }
	public float BulletSpeed { get => bulletSpeed; set => bulletSpeed = value; }
	public int Cost { get => cost; set => cost = value; }
	public float Accuracy { get => accuracy; set => accuracy = value; }
    public float FireRate { get => fireRate; set => fireRate = value; }
    public float Damage { get => damage; set => damage = value; }
    public int CurrentAmmo { get => currentAmmo; set => currentAmmo = value; }
    public int MaxClip { get => maxClip; set => maxClip = value; }
    public float ReloadTime { get => reloadTime; set => reloadTime = value; }
    public int CurrentClipAmmo { get => currentClipAmmo; set => currentClipAmmo = value; }
    internal WeaponType Type { get => type; set => type = value; }
    public float Range { get=> range; set => range = value; }
	public int MaxAmmo { get => maxAmmo; set => maxAmmo = value; }

	private void Start()
	{
        currentAmmo = maxAmmo;
        currentClipAmmo = maxClip;
	}

    public bool refillAmmo()
    {
        if(currentAmmo < maxAmmo)
        {
            currentAmmo = maxAmmo;
            return true;
        }
        return false;
    }

}
