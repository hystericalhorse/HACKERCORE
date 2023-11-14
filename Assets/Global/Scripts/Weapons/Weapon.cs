using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    public float fireDelay = 2; // Number of Seconds in Between Firing
    protected float timer = 0;

    public abstract void OnShoot();

    public virtual void RaycastGun(Vector2 direction, int damage, float rayDistance, int rayCount = 1, float raySpread = 0)
    {
        for (int i = 0; i < rayCount; i++)
        {
            Vector2 spread_dir = direction;
			var quat = Quaternion.AngleAxis(Random.Range(-raySpread, raySpread), Vector3.forward);
			spread_dir += (Vector2)quat.eulerAngles;

            var hits = Physics2D.RaycastAll(transform.position, spread_dir, rayDistance);
            Debug.DrawRay(transform.position, spread_dir * rayDistance, Color.red, 1);
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

    public virtual void ProjectileGun(Vector2 direction, int damage, float prjVelocity, int prjCount = 1, float prjSpread = 0)
    {

    }
}
