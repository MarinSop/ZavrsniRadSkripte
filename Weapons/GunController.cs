using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GunController : MonoBehaviour
{

    private GunStats gunStats;
    private RectTransform reloadUI;

    private bool isReloading = false;
    private float nextShot;
    private float reloadTimeElapsed = 0;
    private Player player;

    private AudioManager audioManager;


    // Start is called before the first frame update
    void Start()
    {
        gunStats = GetComponent<GunStats>();
        nextShot = Time.time;
        //gunStats.CurrentClipAmmo = gunStats.MaxClip;
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        reloadUI = GameObject.FindGameObjectWithTag("reloadUI").transform.GetChild(0).GetComponent<RectTransform>(); ;
        reloadUI.gameObject.SetActive(false);
        audioManager = FindObjectOfType<AudioManager>();
	}

    // Update is called once per frame
    void Update()
    {
        shoot();
        reload();
    }


    private void reload()
    {
        if (isReloading && reloadTimeElapsed >= gunStats.ReloadTime)
        {
            int shotBullets = Mathf.Abs(gunStats.MaxClip - gunStats.CurrentClipAmmo);
            gunStats.CurrentClipAmmo = gunStats.CurrentAmmo < shotBullets ? gunStats.CurrentClipAmmo + gunStats.CurrentAmmo : gunStats.CurrentClipAmmo + shotBullets;
            gunStats.CurrentAmmo -= shotBullets;
            gunStats.CurrentAmmo = gunStats.CurrentAmmo < 0 ? 0 : gunStats.CurrentAmmo;
            isReloading = false;
            reloadTimeElapsed = 0;
            player.updateAmmoUI();
			reloadUI.gameObject.SetActive(false);
		}
        if (gunStats.CurrentAmmo > 0 && ((Input.GetKeyDown(KeyCode.R) && gunStats.CurrentClipAmmo < gunStats.MaxClip) || gunStats.CurrentClipAmmo <= 0) && !isReloading)
        {
            isReloading = true;
			reloadTimeElapsed = 0;
			reloadUI.gameObject.SetActive(true);
            if(gunStats.Type == WeaponType.Secondary)
            {
                audioManager.Play("PistolReload");
            }
            else if(gunStats.Type == WeaponType.Primary)
			{
				audioManager.Play("RifleReload");
			}
		}
        if (isReloading)
        {
            reloadTimeElapsed += Time.deltaTime;
            reloadUI.eulerAngles += new Vector3(0, 0, 50 * Time.deltaTime);
        }
    }


    private void shoot()
    {
        Vector3 lookDir = new Vector3(SimpleInput.GetAxis("LookHorizontal"), 0.0f, SimpleInput.GetAxis("LookVertical"));
		//if (Input.GetButton("Fire1") && Time.time >= nextShot && !isReloading && gunStats.CurrentClipAmmo > 0 && 
		//    !player.CantAttack)
		if (lookDir.magnitude >= 0.1 && Time.time >= nextShot && !isReloading && gunStats.CurrentClipAmmo > 0 && 
		    !player.CantAttack)
		{
			Vector3 startShootingPoint = new Vector3(transform.root.position.x, transform.root.position.y + 1, transform.root.position.z);
            float offset = Random.Range(-1f, 1f) * (1f - gunStats.Accuracy);
            Vector3 shootingDirection = transform.root.forward * gunStats.Range + new Vector3(offset, offset, offset);
            Vector3 hitPosition = Vector3.zero;
            RaycastHit hit;
            if (Physics.Raycast(startShootingPoint, shootingDirection, out hit,gunStats.Range))
            {
                if(hit.collider.gameObject.CompareTag("Enemy"))
                {
                    hit.collider.gameObject.GetComponent<ZombieStats>().takeDamage(gunStats.Damage);
                    hitPosition = hit.point;
                }
                else
                {
					hitPosition = startShootingPoint + transform.root.forward * gunStats.Range + new Vector3(1,0,0);
                }

			}
            else
            {
				hitPosition = startShootingPoint + transform.root.forward * gunStats.Range + new Vector3(1, 0, 0);
			}
            GameObject trail = Instantiate(gunStats.BulletTracer, gunStats.ShootingPoint.position, Quaternion.identity);
            audioManager.PlayOneShot(gunStats.ShotSoundName);
            trail.GetComponent<BulletController>().HitPosition = hitPosition;
            gunStats.CurrentClipAmmo--;
            nextShot = (1.0f / gunStats.FireRate) + Time.time;
            player.updateAmmoUI();
        }
    }

    public bool refillAmmo()
    {
        if(gunStats.refillAmmo())
        {
            player.updateAmmoUI();
            return true;
        }
        return false;
    }

	private void OnDrawGizmos()
	{
        Gizmos.color = Color.red;
        Gizmos.DrawRay(new Vector3(transform.root.position.x, transform.root.position.y + 1, transform.root.position.z)
            , transform.root.forward * gunStats.Range);
	}

}
