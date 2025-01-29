using System;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
	[SerializeField] private GameObject actionCameraGameObject;

	private void Start()
	{
		BaseAction.OnAnyActionStarted += BaseAction_OnAnyActionStarted;
		BaseAction.OnAnyActionCompleted += BaseAction_OnAnyActionCompleted;

		HideActionCamera();
	}

	private void BaseAction_OnAnyActionStarted(object sender, EventArgs e)
	{
		switch (sender)
		{
			case ShootAction shootAction:
				var shootingUnit = shootAction.GetUnit();
				var targetUnit = shootAction.GetTargetUnit();
				var cameraCharacterHeight = Vector3.up * 1.7f;
				var shootDirection = (targetUnit.transform.position - shootingUnit.transform.position).normalized;
				var shoulderPosition = Quaternion.Euler(0, 90, 0) * shootDirection * 0.5f;

				var actionCameraPosition = shootingUnit.transform.position 
					+ cameraCharacterHeight + shoulderPosition - shootDirection;

				actionCameraGameObject.transform.position = actionCameraPosition;
				actionCameraGameObject.transform.LookAt(targetUnit.transform.position + cameraCharacterHeight);

				ShowActionCamera();
				break;
		}

	}

	private void BaseAction_OnAnyActionCompleted(object sender, EventArgs e)
	{
		switch (sender)
		{
			case ShootAction shootAction:
				HideActionCamera();
				break;
		}
	}

	private void ShowActionCamera()
	{
		actionCameraGameObject.SetActive(true);
	}

	private void HideActionCamera()
	{
		actionCameraGameObject.SetActive(false);
	}

}
