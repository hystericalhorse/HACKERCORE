using UnityEngine;

public class ProjectileWeapon : Weapon
{
	[SerializeField] public GameObject projectilePrefab;

	public override void Update() => base.Update();

	public override void OnShoot()
	{
		var parent = gameObject.transform.parent.gameObject;
		var direction = transform.position - parent.transform.position;

		for (int i = 0; i < count; i++)
		{
			Vector2 spread_dir = Quaternion.AngleAxis(Random.Range(-spread, spread), Vector3.forward) * direction;

			var projectile = Instantiate(projectilePrefab);
			projectile.GetComponent<Projectile>().Activate(spread_dir, range, damage);
			projectile.GetComponent<Projectile>().enterCollision +=
			(Collision2D collision) =>
			{
				var isEnemy = collision.gameObject.GetComponent<Enemy>();
				if (isEnemy != null)
				{
					isEnemy.OnHurt();
				}
			};
		}
	}
}

