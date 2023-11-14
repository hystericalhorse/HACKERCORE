using UnityEngine;

public class Grunt : Enemy
{
	[SerializeField] new Collider2D collider;

	#region MonoBehaviour
	void Start()
	{

	}


	void Update()
	{

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
