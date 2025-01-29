using UnityEngine;

public class LookAtCamera : MonoBehaviour
{
	[SerializeField] private bool isInverted = false;

	private Transform mainCameraTransform;
	private void Awake()
	{
		mainCameraTransform = Camera.main.transform;
	}

	private void LateUpdate()
	{
		if (isInverted)
			transform.LookAt(transform.position + mainCameraTransform.rotation * Vector3.forward, mainCameraTransform.rotation * Vector3.up);
		else
			transform.LookAt(mainCameraTransform);
	}
}
