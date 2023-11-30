using System.Collections;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField] protected int damage;
    [SerializeField] protected int range;
    [SerializeField] protected int count;
    [SerializeField] protected float spread;
    [SerializeField] protected float width = 1;

	[SerializeField] protected string firesound;
	[SerializeField] protected string impactsound;

	// Prefabs
	[SerializeField] protected GameObject guide;
	[SerializeField] protected GameObject trail;
	[SerializeField] protected GameObject impact;

	[SerializeField] protected Transform parent;

    [SerializeField] float fireDelay = 2;
    float timer = 0;

	protected Vector2 direction = Vector2.zero;

	public virtual void Awake()
	{
		if (parent == null)
			parent = gameObject.transform.parent.transform;
	}

    public virtual void OnShoot()
    {
		for (int i = 0; i < count; i++)
		{
			Vector2 spread_dir = Quaternion.AngleAxis(Random.Range(-spread, spread), Vector3.forward) * direction;

			var hits = (width > 1)?
				Physics2D.CircleCastAll(transform.position, width * 0.5f, spread_dir, range):
				Physics2D.RaycastAll(transform.position, spread_dir, range);

			var bullet = Instantiate(trail, transform.position, Quaternion.identity);
			// TODO Replace this stuff with a LineRenderer, Do the impact in the hits section.
			StartCoroutine(Trace(bullet, spread_dir));
			if (!string.IsNullOrEmpty(firesound)) GameManager.GM.PlaySound(firesound);

			foreach (var hit in hits)
			{
				var isEnemy = hit.collider.gameObject.GetComponent<Enemy>();
				if (isEnemy != null)
				{
					StopCoroutine(Trace(bullet, spread_dir));
					if (bullet)
					{
						if (impact) Instantiate(impact, hit.transform.position, Quaternion.identity);
						if (!string.IsNullOrEmpty(impactsound)) GameManager.GM.PlaySound(impactsound);
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
		direction = transform.position - parent.position;		

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
