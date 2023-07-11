using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shop : MonoBehaviour
{
    private GameObject shopUI;
    public bool PlayerClose { get; set; }

    // Start is called before the first frame update
    void Start()
    {
        shopUI = GameObject.FindGameObjectWithTag("shopUI");
        shopUI.transform.localScale = Vector3.zero;
        shopUI.SetActive(false);
    }
    public void setPlayerClose(bool state)
    {
        PlayerClose = state;
    }


	private void Update()
	{
		if (PlayerClose && SimpleInput.GetButtonDown("Interact"))
		{
            if (shopUI.activeInHierarchy == true)
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
	}


    public void activateSection(GameObject sectionButton)
    {
        Transform parent = sectionButton.transform.parent;
        for(int i = 0; i < parent.childCount; i++)
        {
            parent.GetChild(i).gameObject.SetActive(false);
        }
        sectionButton.SetActive(true);
    }


}
