using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RoundManager : MonoBehaviour
{

    private ZombiesSpawnManager spawner;
    private TextMeshProUGUI roundUI;
	private TextMeshProUGUI restUI;

	[SerializeField]
    private int runnerZombieRoundSpawn;
	[SerializeField]
	private int tankZombieRoundSpawn;
	[SerializeField]
    private float restTime;

    private float restTimeElapsed = 0;
    private int currentRound = 1;
    private bool isRoundInProgress = false;
    private bool isRestInProgress = true;

    // Start is called before the first frame update
    void Start()
    {
        Application.targetFrameRate = 60;
        spawner = GetComponent<ZombiesSpawnManager>();
        roundUI = GameObject.FindGameObjectWithTag("roundUI").GetComponent<TextMeshProUGUI>();
        roundUI.text = currentRound.ToString();
        restUI = GameObject.FindGameObjectWithTag("restUI").GetComponent<TextMeshProUGUI>();
		restUI.transform.localScale = Vector3.zero;
		LeanTween.scale(restUI.gameObject, Vector3.one, 0.5f);
		restUI.text = "Round starts in\n" + Mathf.Round(restTimeElapsed).ToString();
	}

    // Update is called once per frame
    void Update()
    {
        if(isRestInProgress) 
        {
            restTimeElapsed += Time.deltaTime;
			restUI.text = "Round starts in\n" + (restTime - Mathf.Round(restTimeElapsed)).ToString();
			if (restTimeElapsed >= restTime)
            {
                restTimeElapsed = 0;
                isRestInProgress = false;
                isRoundInProgress = true;
                spawner.startSpawning();
				restUI.text = "Round starts in\n" + (restTime - Mathf.Round(restTimeElapsed)).ToString();
                LeanTween.scale(restUI.gameObject, Vector3.zero, 0.5f).setEase(LeanTweenType.easeOutExpo)
					.setOnComplete(() => { restUI.gameObject.SetActive(false); });
            }
        }
        else if(isRoundInProgress)
        {
            if(spawner.isRoundOver())
            {
                currentRound++;
                roundUI.text = currentRound.ToString();
                isRoundInProgress = false;
                isRestInProgress = true;
                spawner.resetSpawner();
                spawner.increaseZombies();
				restUI.gameObject.SetActive(true);
				LeanTween.scale(restUI.gameObject, Vector3.one, 0.5f);
				if (currentRound == runnerZombieRoundSpawn)
                {
                    spawner.startSpawningRunnerZombie();
                }
                if(currentRound == tankZombieRoundSpawn)
                {
                    spawner.startSpawningTankZombie();
                }
            }
        }
    }
}
