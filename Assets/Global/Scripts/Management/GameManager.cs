using UnityEngine;
using System;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;

public class GameManager : MonoBehaviour
{
	public static GameManager GM { get; private set; }
	public PlayerData Player;

	public float timer = 0;
	public bool isPlaying = false;

	[SerializeField] Canvas hud;
	[SerializeField] Canvas pause;

	[SerializeField] AudioMixerGroup mixerGroup;
	[SerializeField] Sound[] sounds;

	#region MONOBEHAVIOUR
	private void Awake()
	{
		//if (GM is not null && GM != this)
		//{
		//	Destroy(this);
		//	return;
		//}
		//else
		//{
		//	GM = this;
		//}

		GM = this;

		isPlaying = false;
		NewGame();

		foreach (var sound in sounds)
		{
			sound.source = gameObject.AddComponent<AudioSource>();
			sound.source.clip = sound.clip;

			sound.source.volume = sound.volume;
			sound.source.pitch = sound.pitch;
			sound.source.loop = sound.loop;

			sound.source.playOnAwake = false;
			sound.source.outputAudioMixerGroup = mixerGroup;
		}
	}

	private void Update()
	{
		
	}
	#endregion

	public void NewGame()
	{
		Player = new PlayerData();

		PlayGame();
	}

	public void PlayGame()
	{
		UnPauseGame();


	}

	public void PauseGame()
	{
		Time.timeScale = Mathf.Epsilon;
		hud.gameObject.SetActive(false);
		pause.gameObject.SetActive(true);

		isPlaying = false;
	}

	public void UnPauseGame()
	{
		Time.timeScale = 1;
		hud.gameObject.SetActive(true);
		pause.gameObject.SetActive(false);

		isPlaying = true;
	}

    public void GameOver()
	{
		//TODO Serialize highscores and show GameOver UI
	}

	public void Title()
	{
		StopSounds();
		SceneManager.LoadScene("Menu");
	}

	#region Player Manager

	public void DamagePlayer(int damage)
	{
		Player.Health -= (damage - Player.Shield);
		if (Player.Health <= 0) GameOver();
	}

	#endregion

	#region Audio Manager

	public void PlaySound(string name)
	{
		foreach (var sound in sounds)
		{
			if (sound.name == name)
			{
				sound.source.Play();
				return;
			}
		}
	}

	public void StopSounds()
	{
		foreach (var sound in sounds)
		{
			sound.source.Stop();
		}
	}

	#endregion

	[Serializable]
	public struct Highscores
	{
		public string nickname;
		public int highscoreLevel;
		public int highscoreKills;
	}
}