using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{

	[SerializeField]
	private Image transition;
	[SerializeField]
	private GameObject pauseMenu;
	[SerializeField]
	private GameObject shopUI;
	// Start is called before the first frame update
	void Start()
    {
		Color defCol = Color.black;
		Color endCol = defCol;
		endCol.a = 0;
		transition.color = defCol;
		LeanTween.value(transition.gameObject, defCol.a, endCol.a, 0.5f)
			.setOnUpdate((float alpha) =>
			{
				Color color = transition.color;
				color.a = alpha;
				transition.color = color;
			});
	}

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape)) 
		{
			if(pauseMenu.activeInHierarchy) 
			{
				FindObjectOfType<Player>().CantAttack = false;
				pauseMenu.SetActive(false);
			}
			else
			{
				FindObjectOfType<Player>().CantAttack = true;
				pauseMenu.SetActive(true);
			}
		}
    }
	public void resume()
	{
		pauseMenu.SetActive(false);
		FindObjectOfType<Player>().CantAttack = false;
	}

	public void closeShopUi()
	{
		if (shopUI.activeInHierarchy)
		{
			LeanTween.scale(shopUI, Vector3.zero, 0.5f).setEase(LeanTweenType.easeOutExpo)
			.setOnComplete(() =>
			{
				shopUI.SetActive(false);
				FindObjectOfType<Player>().CantAttack = false;
			});
		}
		else
		{
			shopUI.SetActive(true);
			FindObjectOfType<Player>().CantAttack = true;
			LeanTween.scale(shopUI, new Vector3(0.8f, 0.9f, 1), 0.5f);
		}
	}

	public void loadScene(string sceneName)
	{
		Color defCol = transition.color;
		Color endCol = defCol;
		endCol.a = 1.0f;
		LeanTween.value(transition.gameObject, defCol.a, endCol.a, 0.5f)
			.setOnUpdate((float alpha) =>
			{
				Color color = transition.color;
				color.a = alpha;
				transition.color = color;
			}).setOnComplete(() => { SceneManager.LoadScene(sceneName, LoadSceneMode.Single); });
	}
}
