using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour
{
    [SerializeField]
    private bool isAmmo;
	[SerializeField]
	private bool isHealth;

	private void Start()
	{
        LeanTween.moveLocalY(gameObject, transform.localPosition.y + 0.5f, 2.0f).setEaseInOutSine().setLoopPingPong();
		LeanTween.rotateAround(gameObject, Vector3.up, 360f, 5.0f)
	    .setEase(LeanTweenType.linear)
	    .setLoopClamp();
	}

    public void pickingUp(Player player)
    {
			bool isTaken = false;
			if (isAmmo)
			{
				if (player.PrimaryHolder.childCount > 0)
				{
					if (player.PrimaryHolder.GetChild(0).GetComponent<GunController>().refillAmmo())
					{
						isTaken = true;
					}
				}
				if (player.SecondaryHolder.childCount > 0)
				{
					if (player.SecondaryHolder.GetChild(0).GetComponent<GunController>().refillAmmo())
					{
						isTaken = true;
					}
				}

			}
			if (isHealth)
			{
				if (player.Health < player.MaxHealth)
				{
					player.restoreHealth();
					isTaken = true;
				}
			}
			if (isTaken)
			{
				Destroy(gameObject);
			}
		}
}
