using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombiesSpawnManager : MonoBehaviour
{
    [SerializeField]
    private List<Transform> spawnPositions;
    [SerializeField]
    private GameObject regularZombiePrefab;
	[SerializeField]
	private GameObject fastZombiePrefab;
	[SerializeField]
	private GameObject tankZombiePrefab;
    [SerializeField]
    private int regularZombies;
	[SerializeField]
	private int fastZombies;
	[SerializeField]
	private int tankZombies;
    [SerializeField]
    private float spawnMutiplier;
    [SerializeField]
    private int maxSpawnedZombies;
    [SerializeField]
    private float spawnInterval;


    public int currentlySpawnedZombies = 0;
    private int regularZombiesSpawned = 0;
	private int fastZombiesSpawned = 0;
	private int tankZombiesSpawned = 0;
    private bool isSpawning = false;
	private bool isSpawningRunnerZombies = false;
	private bool isSpawningTankZombies = false;




	// Start is called before the first frame update
	void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
            
    }

	public void addSpawnPoints(List<Transform> newSpawnPoints)
	{
		foreach(var s in newSpawnPoints)
		{
			spawnPositions.Add(s);
		}
	}


    public void startSpawning()
    {
        if(!isSpawning)
        {
            isSpawning = true;
            StartCoroutine(spawnRegularZombies());
			if(isSpawningRunnerZombies)
			{
				StartCoroutine(spawnFastZombies());
			}
			if(isSpawningTankZombies)
			{
				StartCoroutine(spawnTankZombies());
			}
		}
    }

    public void stopSpawning()
    {
		if (isSpawning)
		{
			isSpawning = false;
		}
	}

    private IEnumerator spawnRegularZombies()
    {
        while(regularZombiesSpawned < regularZombies)
        {
            if(isSpawning)
            {
                int ranIndex = Random.Range(0, spawnPositions.Count);
                Instantiate(regularZombiePrefab, spawnPositions[ranIndex].position, Quaternion.identity);
                currentlySpawnedZombies++;
                regularZombiesSpawned++;
				if(currentlySpawnedZombies >= maxSpawnedZombies)
				{
					isSpawning = false;
				}
            }

            yield return new WaitForSeconds(spawnInterval);
        }
    }

	private IEnumerator spawnFastZombies()
	{
		while (fastZombiesSpawned < fastZombies)
		{
			if (isSpawning)
			{
				int ranIndex = Random.Range(0, spawnPositions.Count);
				Instantiate(fastZombiePrefab, spawnPositions[ranIndex].position, Quaternion.identity);
				currentlySpawnedZombies++;
				fastZombiesSpawned++;
				if (currentlySpawnedZombies >= maxSpawnedZombies)
				{
					isSpawning = false;
				}
			}

			yield return new WaitForSeconds(spawnInterval);
		}
	}

	private IEnumerator spawnTankZombies()
	{
		while (tankZombiesSpawned < tankZombies)
		{
			if (isSpawning)
			{
				int ranIndex = Random.Range(0, spawnPositions.Count);
				Instantiate(tankZombiePrefab, spawnPositions[ranIndex].position, Quaternion.identity);
				currentlySpawnedZombies++;
				tankZombiesSpawned++;
				if (currentlySpawnedZombies >= maxSpawnedZombies)
				{
					isSpawning = false;
				}
			}

			yield return new WaitForSeconds(spawnInterval);
		}
	}


	public bool isRoundOver()
	{
		int currZomb = 0;
		currZomb += regularZombiesSpawned;
		int maxZombs = 0;
		maxZombs += regularZombies;
		if(isSpawningRunnerZombies) 
		{
			currZomb += fastZombiesSpawned;
			maxZombs += fastZombies;
		}
		if(isSpawningTankZombies)
		{
			currZomb += tankZombiesSpawned;
			maxZombs += tankZombies;
		}
		if(currZomb >= maxZombs && currentlySpawnedZombies <= 0)
		{
			return true;
		}
		return false;
	}

	public void resetSpawner()
	{
		regularZombiesSpawned = 0;
		fastZombiesSpawned = 0;
		tankZombiesSpawned = 0;
		currentlySpawnedZombies= 0;
		isSpawning= false;
	}
	
	public void increaseZombies()
	{
		regularZombies = (int)Mathf.Round(regularZombies * spawnMutiplier);
		if(isSpawningRunnerZombies)
			fastZombies = (int)Mathf.Round(fastZombies * spawnMutiplier);
		if(isSpawningTankZombies)
			tankZombies = (int)Mathf.Round(tankZombies * spawnMutiplier);
	}

	public void startSpawningRunnerZombie()
	{
		isSpawningRunnerZombies = true;
	}

	public void startSpawningTankZombie()
	{
		isSpawningTankZombies = true;
	}

	public void zombieDied()
    {
		currentlySpawnedZombies--;
		if (currentlySpawnedZombies < maxSpawnedZombies)
		{
			isSpawning = true;
		}
	}
}
