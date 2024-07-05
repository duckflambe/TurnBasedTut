using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinAction : BaseAction
{
	[SerializeField]
	private float spinSpeed = 360.0f;

	private float spinRemaining = 0.0f;

	public override string GetActionName()
	{
		return "Spin";
	}

	private void Update()
	{
		if (!isActive)
		{
			return;
		}

		float distance = spinSpeed * Time.deltaTime;
		if (spinRemaining < distance)
		{
			transform.Rotate(Vector3.up, spinRemaining);
			isActive = false;
			onComplete();
		}
		else
		{
			transform.Rotate(Vector3.up, distance);
			spinRemaining -= distance;
		}
	}

	public override void Act(GridPosition gridPosition, Action onActionComplete)
	{
		this.onComplete = onActionComplete;
		isActive = true;
		spinRemaining = 360.0f;
	}

	public override List<GridPosition> GetValidActionGridPositions()
	{
		return new List<GridPosition>
		{
			unit.GetGridPosition()
		};
	}
}
