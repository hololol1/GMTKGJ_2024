using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InGameMenu : MonoBehaviour
{
	public void LoadMenu()
	{
		StartCoroutine(DelaySceneLoad());
	}

	IEnumerator DelaySceneLoad()
	{
		yield return new WaitForSeconds(0.1f);
		SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
	}

	public void LoadMainGame1()
	{
		StartCoroutine(DelaySceneLoadMainGame1());
	}

	IEnumerator DelaySceneLoadMainGame1()
	{
		yield return new WaitForSeconds(0.1f);
		SceneManager.LoadScene("MainGameLevel1", LoadSceneMode.Single);
	}

	public void LoadMainGame2()
	{
		StartCoroutine(DelaySceneLoadMainGame2());
	}

	IEnumerator DelaySceneLoadMainGame2()
	{
		yield return new WaitForSeconds(0.1f);
		SceneManager.LoadScene("MainGameLevel2", LoadSceneMode.Single);
	}
}
