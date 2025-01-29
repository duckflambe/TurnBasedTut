using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveAction : BaseAction
{
	public event EventHandler OnStartMoving;
	public event EventHandler OnStopMoving;

	[SerializeField]
	private int maxMoveDistance = 4;
	
	[SerializeField]
	private float moveSpeed = 4.0f;

	[SerializeField]
	private float rotationSpeed = 10.0f;

	private List<Vector3> moveTargetPositions;
	private int moveTargetIndex = 0;

	public override string GetActionName()
	{
		return "Move";
	}

	void Update()
	{
		if (!isActive)
		{
			return;
		}

		var moveDirection = moveTargetPositions[moveTargetIndex] - transform.position;
		transform.forward = Vector3.Lerp(transform.forward, moveDirection.normalized, Time.deltaTime * rotationSpeed);

		if (moveDirection.magnitude > 0.1f)
		{
			transform.position += moveDirection.normalized * Time.deltaTime * moveSpeed;
		}
		else
		{
			moveTargetIndex++;
			if (moveTargetIndex >= moveTargetPositions.Count)
			{
				OnStopMoving?.Invoke(this, EventArgs.Empty);
				ActionComplete();
				return;
			}
		}
	}

	public override void Act(GridPosition gridPosition, Action onActionComplete)
	{
		moveTargetPositions = new List<Vector3>();
		moveTargetIndex = 0;

		var (pathList, _) = Pathfinding.Instance.FindPath(unit.GetGridPosition(), gridPosition);

		foreach (var path in pathList)
		{
			moveTargetPositions.Add(LevelGrid.Instance.GetWorldPosition(path));
		}

		OnStartMoving?.Invoke(this, EventArgs.Empty);

		ActionStart(onActionComplete);
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
					&& !LevelGrid.Instance.IsOccupiedGridPosition(testGridPosition)
					&& Pathfinding.Instance.HasPath(unitGridPosition, testGridPosition, maxMoveDistance))
				{
					validGridPositions.Add(testGridPosition);
				}
			}
		}
		return validGridPositions;
	}

	public override EnemyAIAction GetEnemyAIAction(GridPosition gridPosition)
	{
		var targetCount = unit.GetAction<ShootAction>().GetTargetCount(gridPosition);

		return new EnemyAIAction
		{
			actionScore = Math.Min(99, 10 * targetCount),
			gridPosition = gridPosition
		};
	}

}
