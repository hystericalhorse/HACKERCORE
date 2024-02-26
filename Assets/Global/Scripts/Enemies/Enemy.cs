using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
public abstract class Enemy : MonoBehaviour
{
    public float speed;

	public float attackRange;
	public int attackDamage;
	public float attackCooldown;
	float attacKTimer;

    public int health;
    public bool alive;

	public int xpValue;

    private Rigidbody2D rb;
    private Collider2D col;

    public delegate void onDeath();
    public onDeath callOnDeath;

	public void Awake()
	{
		rb = GetComponent<Rigidbody2D>();
		col = GetComponent<Collider2D>();
	}

	public void Start()
	{
		alive = true;
		attacKTimer = attackCooldown;
	}

	public void Update()
	{
		if (!alive) return;
		if (health <= 0 && alive)
		{
			OnDeath();
			return;
		}

        Follow();

		if (attacKTimer > 0) attacKTimer -= Time.deltaTime;
	}

	public virtual void Follow()
    {
        Transform target = GameObject.FindGameObjectWithTag("Player").transform;

		Vector3 dir = target.position - transform.position;
		float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

		transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

		if (Vector2.Distance(transform.position, target.position) - (col as CircleCollider2D).radius <= attackRange)
		{
			if (attacKTimer <= 0) Attack();
		}
		else
		{
			var movement = target.position - transform.position;
			rb.velocity = movement.normalized * speed;
		}
	}

    public virtual void Attack()
	{
		Transform target = GameObject.FindGameObjectWithTag("Player").transform;
		Vector3 dir = target.position - transform.position;

		var hitscan = Physics2D.RaycastAll(transform.position, dir.normalized, attackRange + (col as CircleCollider2D).radius);
		Debug.DrawRay(transform.position, dir.normalized * (attackRange + (col as CircleCollider2D).radius), Color.cyan);
		foreach (var hit in hitscan)
		{
			if (hit.collider == null) continue;
			if (hit.collider.gameObject == target.gameObject)
			{
				GameManager.GM.DamagePlayer(attackDamage);
				attacKTimer = attackCooldown;
				return;
			}
		}
	}

    public virtual void OnHurt(int damage)
	{
		health -= damage;
	}

    public virtual void OnDeath()
    {
        callOnDeath();

        alive = false;
        Destroy(gameObject);
    }
}
