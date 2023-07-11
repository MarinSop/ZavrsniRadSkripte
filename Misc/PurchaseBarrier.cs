using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PurchaseBarrier : MonoBehaviour
{

    [SerializeField]
    private int cost;
    [SerializeField]
    private List<Transform> spawnPoints;
    [SerializeField]
    private GameObject CostInfo;

	private void Start()
	{
        CostInfo.GetComponentInChildren<TextMeshProUGUI>().text = "Buy For \n" + cost;
        CostInfo.SetActive(false);
	}
	public void purchase(Player player)
    {
        if(cost <= player.Points)
        {
            player.buy(cost);
            if(spawnPoints.Count > 0)
            {
                ZombiesSpawnManager man = FindObjectOfType<ZombiesSpawnManager>();
                foreach(var s in spawnPoints)
                {
                    man.addSpawnPoints(spawnPoints);

                }
            }
            Destroy(gameObject);
        }
    }

    public void buyEnd(Player player)
    {
        if(player.Points >= cost)
        {
            player.buy(cost);
            FindObjectOfType<PauseMenu>().loadScene("MainMenu");
        }
    }

    public void DeOrActiveCostInfo()
    {
        if(CostInfo.activeInHierarchy) 
        {
            CostInfo.SetActive(false);
        }
        else
        {
            CostInfo.SetActive(true);
        }
    }
}
