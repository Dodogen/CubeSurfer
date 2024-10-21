using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameMenuController : MonoBehaviour
{
    [SerializeField] private List<GameObject> _playTimeObjects = new List<GameObject>();
    [SerializeField] private List<GameObject> _pauseTimeObjects = new List<GameObject>();

	private void Start()
	{
        SetAbleStatusForAll(_pauseTimeObjects, false); 
        SetAbleStatusForAll(_playTimeObjects, true);
	}

	public void OnContinueButtonClick()
    {
        Debug.Log("continue button clicked");
		SetAbleStatusForAll(_pauseTimeObjects, false);
		SetAbleStatusForAll(_playTimeObjects, true);
		Time.timeScale = 1.0f;

	}

	public void OnPauseButtonClick()
    {
		Debug.Log("pause button clicked");
		SetAbleStatusForAll(_pauseTimeObjects, true);
		SetAbleStatusForAll(_playTimeObjects, false);
		Time.timeScale = 0f;

	}

	public void OnRestartButtonClick()
    {
		Debug.Log("restart button clicked");
		SceneManager.LoadScene("PlayScene");
        Time.timeScale = 1.0f;
    }

    private void SetAbleStatusForAll(List<GameObject> objects, bool value)
    {
        foreach (var obj in objects) 
        {
            obj.SetActive(value);
        }
    }
}
