using UnityEngine;
using System;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
using TMPro;
using System.Collections;

public class GameManager : MonoBehaviour
{
	public static GameManager GM { get; private set; }
	[Header("Player Data")]
	public PlayerData Player;
	public float invulnTimer = 0;
	public float shieldRegenTimer = 0;

	[Header("Game State")]
	public float timer = 0;
	public bool isPlaying = false;

	[Header("UI")]
	[SerializeField] Canvas hud;
	[SerializeField] Canvas pause;
	[SerializeField] TextMeshProUGUI pauseDesc;
	[SerializeField] TextMeshProUGUI info;
	[SerializeField] GameObject resumeBtn;

	[Header("Audio")]
	[SerializeField] AudioMixerGroup mixerGroup;
	[SerializeField] Sound[] sounds;
	[SerializeField] Sound[] music;
	int musicIndex = 0;

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

		foreach (var sound in music)
		{
			sound.source = gameObject.AddComponent<AudioSource>();
			sound.source.clip = sound.clip;

			sound.source.volume = sound.volume;
			sound.source.pitch = sound.pitch;
			sound.source.loop = sound.loop;

			sound.source.playOnAwake = false;
			sound.source.outputAudioMixerGroup = mixerGroup;
		}

		musicIndex = UnityEngine.Random.Range(0, music.Length - 1);
		Music();
	}

	private void Update()
	{
		if (!isPlaying) return;

		if (invulnTimer > 0) invulnTimer -= Time.deltaTime;

		if (shieldRegenTimer > 0)
			shieldRegenTimer -= Time.deltaTime;
		else if (shieldRegenTimer <= 0 && shieldRegenTimer > -1)
			RegenShield();
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

		GameObject.FindObjectOfType<HealthDisplay>()?.UpdateHealthDisplay();
		Player.NextLevel = (int) Math.Pow(2, Player.Level);

	}

	public void PauseGame()
	{
		pauseDesc.text = "Game Paused!";
		var wave = GameObject.FindAnyObjectByType<EnemyManager>()?.wave - 1;
		info.text =
			$"Waves: {wave}" +
			$"\nKills: {Player.Kills}" +
			$"\nLevel: {Player.Level}";

		Time.timeScale = Mathf.Epsilon;
		hud.gameObject.SetActive(false);
		pause.gameObject.SetActive(true);
		resumeBtn.gameObject.SetActive(true);

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
		pauseDesc.text = "Game Over!";
		var wave = GameObject.FindAnyObjectByType<EnemyManager>()?.wave - 1;
		info.text =
			$"Waves: {wave}" +
			$"\nKills: {Player.Kills}" +
			$"\nLevel: {Player.Level}";

		Time.timeScale = Mathf.Epsilon;
		hud.gameObject.SetActive(false);
		pause.gameObject.SetActive(true);
		resumeBtn.gameObject.SetActive(false);

		isPlaying = false;
	}

	public void Title()
	{
		StopSounds();
		SceneManager.LoadScene("Menu");
	}

	#region Player Manager

	[ContextMenu("Damage")]
	public void DamageTest()
	{
		DamagePlayer(10);
	}

	[ContextMenu("XP")]
	public void ExpText()
	{
		GiveXP(2);
	}

	public void DamagePlayer(int damage)
	{
		if (invulnTimer > 0) return;
		if (Player.Shield > 0)
		{
			GameObject.FindObjectOfType<ShieldDisplay>()?.DamageShield();
			Player.Shield -= 1;
			invulnTimer = Player.InvAfterShieldHit;
			shieldRegenTimer = Player.ShieldRegenTime;
		}
		else
		{
			Player.Health -= damage;
			invulnTimer = Player.InvAfterHit;
			if (Player.Health <= 0) GameOver();
			else GameObject.FindObjectOfType<HealthDisplay>()?.UpdateHealthDisplay();
		}
	}

	public void RegenShield()
	{
		shieldRegenTimer = -1;
		Player.Shield = (int) Mathf.Clamp(Player.Shield + 1, 0, 4);
		GameObject.FindObjectOfType<ShieldDisplay>()?.FixShield();
	}

	public void HealPlayer()
	{
		Player.Health = Player.MaxHealth;
	}

	public void GiveXP(int amount = 0)
	{
		Player.XP += amount;
		if (Player.XP >= Player.NextLevel) LevelUp();
		GameObject.FindObjectOfType<XPDisplay>()?.UpdateXPDisplay();
	}

	public void LevelUp()
	{
		Player.XP = Player.NextLevel - Player.XP;
		Player.Level++;
		//TODO Upgrades
		Player.Speed += 2; // Placeholder Upgrade
		Player.NextLevel = (int)Math.Pow(2, Player.Level);
	}

	#endregion

	#region Audio Manager

	public void Music()
	{
		var song = music[musicIndex];
		if (song == null) return;
		
		StartCoroutine(PlaySong(song, song.source.clip.length - 0.2f, 0.2f));
	}

	public IEnumerator PlaySong(Sound sound, float time, float fadeTime = 0.2f)
	{
		GameObject.FindAnyObjectByType<MusicDisplay>()?.UpdateDisplay(sound.name);
		sound.source.Play();
		sound.source.pitch = Mathf.Epsilon;
		while (sound.source.pitch < sound.pitch)
		{
			sound.source.pitch += fadeTime * Time.deltaTime;
			yield return null;
		}
		sound.source.pitch = sound.pitch;

		yield return new WaitForSeconds(time - 0.2f);

		while (sound.source.pitch > 0)
		{
			sound.source.pitch -= fadeTime * Time.deltaTime;

			yield return null;
		}

		sound.source.Stop();
		sound.source.pitch = sound.pitch;
		musicIndex++; if (musicIndex > music.Length - 1) musicIndex = 0;

		Music();
	}

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

	public IEnumerator FadeInSound(Sound sound, float time = 0.2f)
	{
		sound.source.Play();
		sound.source.pitch = 0;
		for (float i = time; i > 0;)
		{
			sound.source.pitch = Mathf.Lerp(sound.source.pitch, sound.pitch, Time.deltaTime * 0.2f);

			i -= Time.deltaTime;
			yield return null;
		}
	}

	public IEnumerator FadeOutSound(Sound sound, float time = 0.2f)
	{
		sound.source.pitch = 0;
		for (float i = time; i > 0;)
		{
			sound.source.pitch = Mathf.Lerp(sound.source.pitch, 0, Time.deltaTime * 0.2f);

			i -= Time.deltaTime;
			yield return null;
		}

		sound.source.pitch = sound.pitch;
		sound.source.Stop();
	}

	public void StopSounds()
	{
		foreach (var sound in sounds)
		{
			sound.source.Stop();
		}

		foreach (var sound in music)
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