using UnityEngine;

public class CameraController : MonoBehaviour
{
	[SerializeField] Camera Cam;
	[SerializeField] Transform TargetPosition;

	[SerializeField]
	[Range(1, 10)] float Zoom = 5;
	[SerializeField] bool FixedCamera = false;
	[SerializeField]
	[Range(0.1f, 0.2f)] float SlideFactor = 0.1f;

	#region Monobehavior

	private void Awake()
	{
		if (Cam is null) Cam = GetComponent<Camera>();
	}

	private void Update()
	{
		if (FixedCamera)
			transform.position = TargetPosition.position - (Vector3.forward);
	}

	private void FixedUpdate()
	{
		if (Cam.orthographicSize != Zoom) Cam.orthographicSize = Zoom;
		if (!FixedCamera)
			transform.position = Vector3.Slerp(transform.position, TargetPosition.position - Vector3.forward, SlideFactor);
	}

	#endregion
}
