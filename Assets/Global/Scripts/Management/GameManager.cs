using UnityEngine;
using System;

public class GameManager : MonoBehaviour
{
	public static GameManager GM { get; private set; }
	public PlayerData Player;

	[SerializeField] Canvas canvas;

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

		NewGame();
	}

	private void Update()
	{
		
	}
	#endregion

	public void NewGame()
	{
		Player = new PlayerData();
	}

	[Serializable]
	public struct GameData
	{

	}
}