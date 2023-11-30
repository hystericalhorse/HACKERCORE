using UnityEngine;
using System;

public class GameManager : MonoBehaviour
{
	public static GameManager GM { get; private set; }
	public PlayerData Player;

	public float timer = 0;
	public bool isPlaying = false;

	[SerializeField] Canvas hud;
	[SerializeField] Canvas pause;

	#region MONOBEHAVIOUR
	private void Awake()
	{
		if (GM is not null && GM != this)
		{
			Destroy(this);
			return;
		}
		else
		{
			GM = this;
		}

		isPlaying = false;
		NewGame();
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

	#region Player Manager

	public void DamagePlayer(int damage)
	{
		Player.Health -= (damage - Player.Shield);
		if (Player.Health <= 0) GameOver();
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