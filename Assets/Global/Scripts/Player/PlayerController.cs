using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] Joystick move;
    [SerializeField] Joystick look;

    Rigidbody2D rb;

    Vector2 Translation;
    float Rotation;

	#region MonoBehaviour
	void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        GetTranslation();
        GetRotation();

        Move();
        Look();
    }
    #endregion

    void GetTranslation()
    {
        Translation = Vector2.zero;
        Translation = move.Direction.normalized;
    }

    void GetRotation()
    {
        if (look.Direction != Vector2.zero)
        {
			Rotation = Mathf.Atan2(look.Direction.y, look.Direction.x) * 180 / Mathf.PI;
        }
    }

	void Move()
	{
        rb.velocity = Translation * GameManager.GM.Player.Speed;
	}

    void Look()
    {
        transform.rotation = Quaternion.AngleAxis(Rotation, Vector3.forward);
    }
}
