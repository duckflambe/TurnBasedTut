using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Cinemachine;

public class CameraController : MonoBehaviour
{
	[SerializeField]
	CinemachineCamera cinemachineCamera;
	CinemachineFollow cinemachineFollow;

	[SerializeField]
	private float moveSpeed = 20f;

	[SerializeField]
	private float rotateSpeed = 50f;

	[SerializeField]
	private float zoomSpeed = 50f;

	private const float MIN_FOLLOW_Y_OFFSET = 2f;
	private const float MAX_FOLLOW_Y_OFFSET = 12f;
	private Vector3 targetFollowOffset;

	private void Start()
	{
		cinemachineFollow = cinemachineCamera.GetComponent<CinemachineFollow>();
		targetFollowOffset = new Vector3(0, MAX_FOLLOW_Y_OFFSET, 0);
	}

	private void Update()
	{
		HandleMovement();
		HandleRotation();
		HandleZoom();
	}

	private void HandleMovement()
	{
		var moveInput = InputManager.Instance.GetCameraMoveVector();

		Vector3 moveVector = transform.forward * moveInput.y + transform.right * moveInput.x;
		transform.position += moveVector * moveSpeed * Time.deltaTime;
	}

	private void HandleRotation()
	{
		Vector3 rotateInput = new Vector3(0, 
			InputManager.Instance.GetCameraRotate(), 0);

		transform.eulerAngles += rotateInput * rotateSpeed * Time.deltaTime;
	}

	private void HandleZoom()
	{
		float zoomIncreaseAmount = 1f;
		targetFollowOffset.y -= InputManager.Instance.GetCameraZoom() * zoomIncreaseAmount;

		targetFollowOffset.y = Mathf.Clamp(targetFollowOffset.y, MIN_FOLLOW_Y_OFFSET, MAX_FOLLOW_Y_OFFSET);

		cinemachineFollow.FollowOffset.y = Mathf.Lerp(cinemachineFollow.FollowOffset.y, targetFollowOffset.y, Time.deltaTime * zoomSpeed);

		//Debug.Log(cinemachineFollow.FollowOffset.z);
	}


}
