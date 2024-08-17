using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuButtons : MonoBehaviour
{
	[SerializeField]
	private AudioMixer audioMixer;

	public Slider audioSlider;

	private void Start()
	{
		float value;
		bool hasValue = audioMixer.GetFloat("Master", out value);
		if (hasValue)
		{
			audioSlider.value = Mathf.Pow(10, value / 20);
		}
	}

	public void Quit()
	{
		Application.Quit();
	}

	public void StartGame()
	{
		SceneManager.LoadScene("MainGame", LoadSceneMode.Single);
	}

	public void SetVolume()
	{
		audioMixer.SetFloat("Master", Mathf.Log10(audioSlider.value) * 20);
	}

	public float GetMasterLevel()
	{
		float value;
		bool result = audioMixer.GetFloat("Master", out value);
		if (result)
		{
			return value;
		}
		else
		{
			return 0f;
		}
	}
}
