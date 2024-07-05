using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
	[SerializeField]
	CinemachineVirtualCamera virtualCamera;

	[SerializeField]
	private float moveSpeed = 20f;

	[SerializeField]
	private float rotateSpeed = 50f;

	[SerializeField]
	private float zoomSpeed = 50f;

	private const float MIN_FOLLOW_Y_OFFSET = 2f;
	private const float MAX_FOLLOW_Y_OFFSET = 12f;
	private CinemachineTransposer cameraTransposer;
	private Vector3 targetFollowOffset;

	private void Start()
	{
		cameraTransposer = virtualCamera.GetCinemachineComponent<CinemachineTransposer>();
		targetFollowOffset = cameraTransposer.m_FollowOffset;
	}

	private void Update()
	{
		HandleMovement();
		HandleRotation();
		HandleZoom();
	}

	private void HandleMovement()
	{
		Vector3 moveVector = transform.forward * Input.GetAxis("Vertical") + transform.right * Input.GetAxis("Horizontal");
		transform.position += moveVector * moveSpeed * Time.deltaTime;
	}

	private void HandleRotation()
	{
		Vector3 rotateInput = new Vector3(0, 0, 0);
		if (Input.GetKey(KeyCode.Q))
		{
			rotateInput.y = 1f;
		}
		if (Input.GetKey(KeyCode.E))
		{
			rotateInput.y = -1f;
		}

		transform.eulerAngles += rotateInput * rotateSpeed * Time.deltaTime;
	}

	private void HandleZoom()
	{
		float zoomIncreaseAmount = 1f;
		targetFollowOffset.y -= Input.GetAxis("Mouse ScrollWheel") * zoomIncreaseAmount;

		targetFollowOffset.y = Mathf.Clamp(targetFollowOffset.y, MIN_FOLLOW_Y_OFFSET, MAX_FOLLOW_Y_OFFSET);

		cameraTransposer.m_FollowOffset =
			Vector3.Lerp(cameraTransposer.m_FollowOffset, targetFollowOffset, Time.deltaTime * zoomSpeed);
	}


}
