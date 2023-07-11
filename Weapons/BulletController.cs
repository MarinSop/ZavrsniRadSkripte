using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
	float time;
	Vector3 startPosition;
	private Vector3 hitPosition;
	TrailRenderer trail;

	public Vector3 HitPosition { get => hitPosition; set => hitPosition = value; }
	// Start is called before the first frame update
	void Start()
    {
		time = 0;
		startPosition = transform.position;
		trail = GetComponent<TrailRenderer>();
		Destroy(gameObject, trail.time);
    }


    // Update is called once per frame
    void Update()
    {
		if (time < 1)
		{
			transform.position = Vector3.Lerp(startPosition, hitPosition, time);
			time += Time.deltaTime / trail.time;
		}
	}
}
