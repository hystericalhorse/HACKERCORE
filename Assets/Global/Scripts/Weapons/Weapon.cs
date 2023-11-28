using System.Collections;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField] protected int damage;
    [SerializeField] protected int range;
    [SerializeField] protected int count;
    [SerializeField] protected float spread;

	[SerializeField] protected GameObject tracer;
	[SerializeField] protected GameObject impact;

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
			var bullet = Instantiate(tracer, transform.position, Quaternion.identity);
			StartCoroutine(Trace(bullet, spread_dir));

			foreach (var hit in hits)
			{
				var isEnemy = hit.collider.gameObject.GetComponent<Enemy>();
				if (isEnemy != null)
				{
					StopCoroutine(Trace(bullet, spread_dir));
					if (bullet)
					{
						Instantiate(impact, hit.transform.position, Quaternion.identity);
						Destroy(bullet);
					}
					isEnemy.OnHurt(damage);
					break;
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

	public virtual IEnumerator Trace(GameObject bullet, Vector2 dir)
	{
		float dist = range;
		while (dist > 0)
		{
			if (bullet)
				bullet.transform.position += (Vector3) dir.normalized * Time.deltaTime * 100;
			dist -= Time.deltaTime * 100;
			yield return null;
		}

		Destroy(bullet);
	}
}
