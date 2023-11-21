using UnityEngine;

public class Grunt : Enemy
{
	[SerializeField] new Collider2D collider;

	#region MonoBehaviour
	void Start()
	{
		alive = true;
	}


	void Update()
	{
		if (!alive) return;
		if (health <= 0 && alive)
		{
			OnDeath();
			return;
		}
	}
	#endregion

	public override void Attack()
	{
		
	}

	public override void OnHurt()
	{
		Debug.Log("OW!");
	}
}
