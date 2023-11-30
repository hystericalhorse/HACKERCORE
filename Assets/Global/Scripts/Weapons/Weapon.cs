using System.Collections;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField] protected int damage;
    [SerializeField] protected int range;
    [SerializeField] protected int count;
    [SerializeField] protected float spread;
    [SerializeField] protected float width = 1;

	[SerializeField] protected LineRenderer[] tracers;
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

			foreach (var hit in hits)
			{
				var isEnemy = hit.collider.gameObject.GetComponent<Enemy>();
				if (isEnemy != null)
				{
					StopCoroutine(Trace(bullet, spread_dir));
					if (bullet)
					{
						if (impact) Instantiate(impact, hit.transform.position, Quaternion.identity);
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
		Guide();
		direction = transform.position - parent.position;		

		if (timer <= 0)
		{
			OnShoot();
			timer = fireDelay;
		}

		timer -= Time.deltaTime;
	}

	public virtual void Guide()
	{
		Vector2 left_bound = Quaternion.AngleAxis(spread, Vector3.forward) * direction * range;
		Vector2 right_bound = Quaternion.AngleAxis(-spread, Vector3.forward) * direction * range;

		//Debug.DrawRay(transform.position, left_bound, Color.cyan);
		//Debug.DrawRay(transform.position, direction * range, Color.red);
		//Debug.DrawRay(transform.position, right_bound, Color.cyan);
		//
		//Debug.DrawRay(transform.position + (Vector3)left_bound, Vector3.up, Color.green);

		foreach (var tracer in tracers)
		{
			var line = transform.position + (Vector3) (direction * range);
			Vector3[] guidelines = { transform.position, line };
			tracer.SetPositions(guidelines);
		}

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
