using UnityEngine;

public class GameManager : MonoBehaviour
{
	public static GameManager GM { get; private set; }
	public PlayerData Player;

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
	}

	private void Update()
	{
		
	}
	#endregion

	public void NewGame()
	{
		Player = new PlayerData();
	}


}