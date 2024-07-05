using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveAction : BaseAction
{
	[SerializeField]
	private Animator animator;

	[SerializeField]
	private int maxMoveDistance = 4;
	
	[SerializeField]
	private float moveSpeed = 4.0f;

	[SerializeField]
	private float rotationSpeed = 10.0f;

	private Vector3 moveTargetPosition;

	public override string GetActionName()
	{
		return "Move";
	}

	protected override void Awake()
	{
		base.Awake();
		moveTargetPosition = transform.position;
	}

	void Update()
	{
		if (!isActive)
		{
			return;
		}

		var moveDirection = moveTargetPosition - transform.position;

		if (moveDirection.magnitude > 0.1f)
		{
			transform.position += moveDirection.normalized * Time.deltaTime * moveSpeed;
			animator.SetBool("IsMoving", true);
		}
		else
		{
			animator.SetBool("IsMoving", false);
			isActive = false;
			onComplete();
		}

		transform.forward = Vector3.Lerp(transform.forward, moveDirection.normalized, Time.deltaTime * rotationSpeed);
	}

	public override void Act(GridPosition gridPosition, Action onActionComplete)
	{
		this.onComplete = onActionComplete;
		moveTargetPosition = LevelGrid.Instance.GetWorldPosition(gridPosition);
		isActive = true;
	}

	public override List<GridPosition> GetValidActionGridPositions()
	{
		List<GridPosition> validGridPositions = new List<GridPosition>();
		var unitGridPosition = unit.GetGridPosition();
		for (int x = -maxMoveDistance; x <= maxMoveDistance; x++)
		{
			for (int z = -maxMoveDistance; z <= maxMoveDistance; z++)
			{
				var testGridPosition = new GridPosition(unitGridPosition.x + x, unitGridPosition.z + z);
				if ((unitGridPosition != testGridPosition)
					&& LevelGrid.Instance.IsValidGridPosition(testGridPosition)
					&& !LevelGrid.Instance.IsOccupiedGridPosition(testGridPosition))
				{
					validGridPositions.Add(testGridPosition);
				}
			}
		}
		return validGridPositions;
	}
}
