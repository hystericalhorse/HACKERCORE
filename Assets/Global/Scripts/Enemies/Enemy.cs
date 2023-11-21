using UnityEngine;
using UnityEngine.Events;

public abstract class Enemy : MonoBehaviour
{
    public int health;
    public bool alive;

    public delegate void onDeath();
    public onDeath callOnDeath;

    public abstract void Attack();
    public abstract void OnHurt();
    public void OnDeath()
    {
        callOnDeath();

        alive = false;
        Destroy(gameObject);
    }
}
