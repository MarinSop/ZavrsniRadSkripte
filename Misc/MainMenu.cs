using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static System.TimeZoneInfo;

public class MainMenu : MonoBehaviour
{
    [SerializeField]
    private Image transition;

    private GameObject levelSelection;

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
            }).setOnComplete(() => { SceneManager.LoadScene(sceneName,LoadSceneMode.Single); });
    }

    public void exitTheGame()
    {
        Application.Quit();
    }

    public void openLevelSelection(GameObject levelSelection)
    {
		levelSelection.SetActive(true);
    }

	public void closeLevelSelection(GameObject levelSelection)
	{
		levelSelection.SetActive(false);
	}
}
