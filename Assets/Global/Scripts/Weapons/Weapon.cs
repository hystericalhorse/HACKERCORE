using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField] protected int damage;
    [SerializeField] protected int range;
    [SerializeField] protected int count;
    [SerializeField] protected float spread;

    [SerializeField] float fireDelay = 2;
    float timer = 0;

	Vector2 direction = Vector2.zero;

    public virtual void OnShoot()
    {
		var parent = gameObject.transform.parent.gameObject;
		direction = transform.position - parent.transform.position;

		for (int i = 0; i < count; i++)
		{
			Vector2 spread_dir = Quaternion.AngleAxis(Random.Range(-spread, spread), Vector3.forward) * direction;

			var hits = Physics2D.RaycastAll(transform.position, spread_dir, range);
			Debug.DrawRay(transform.position, spread_dir * range, Color.red, 1);
			foreach (var hit in hits)
			{
				var isEnemy = hit.collider.gameObject.GetComponent<Enemy>();
				if (isEnemy != null)
				{
					isEnemy.OnHurt();
				}
			}
		}
	}

	public virtual void Update()
	{
		if (timer <= 0)
		{
			OnShoot();
			timer = fireDelay;
		}

		timer -= Time.deltaTime;
	}
}
