using System.Threading;
using UnityEngine;

public class Pistol : Weapon
{
	public override void OnShoot()
	{
        var parent = gameObject.transform.parent.gameObject;
        var direction = transform.position - parent.transform.position;
		RaycastGun(direction.normalized, 1, 20, 1, 0);
	}
    
    void Update()
    {
        if (timer <= 0)
        {
            OnShoot();
            timer = fireDelay;
        }

        timer -= Time.deltaTime;
    }
}
