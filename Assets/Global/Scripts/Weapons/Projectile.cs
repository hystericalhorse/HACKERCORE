using UnityEngine;
using UnityEngine.UIElements;

[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
public class Projectile : MonoBehaviour
{
    public int damage;
    public Rigidbody2D rb;
	public Collider2D col;

	public bool active = false;
	public float lifetime;
	float timer = 0;

	public delegate void OnCollision(Collision2D other);
	public OnCollision enterCollision;

	public void Awake() => active = false;

	public void Update()
	{
		if (!active) return;

		if (timer <= 0)
		{
			Destroy(gameObject);
		}

		timer -= Time.deltaTime;
	}

	public void Activate(Vector2 direction, float velocity, int damage)
	{
		if (rb == null) rb = gameObject.GetComponent<Rigidbody2D>();
		this.damage = damage;
		rb.velocity = direction * velocity;

		active = true;
		timer = lifetime;
	}

	public void OnCollisionEnter2D(Collision2D collision)
	{
		enterCollision(collision);
	}
}
