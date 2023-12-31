using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] Joystick move;
    [SerializeField] Joystick look;

    Vector2 Translation;
    float Rotation;

	#region MonoBehaviour
	void Start()
    {
        
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
        transform.Translate(Translation * Time.deltaTime * GameManager.GM.Player.Speed, Space.World);
	}

    void Look()
    {
        transform.rotation = Quaternion.AngleAxis(Rotation, Vector3.forward);
    }
}
