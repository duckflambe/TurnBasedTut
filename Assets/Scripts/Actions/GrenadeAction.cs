using System;
using System.Collections.Generic;
using UnityEngine;

public class GrenadeAction : BaseAction
{
	[SerializeField] private Transform grenadePrefab;
	[SerializeField] private int maxShootDistance = 5;
	[SerializeField] private LayerMask obstacleLayerMask;

	public override void Act(GridPosition gridPosition, Action onActionComplete)
	{
		var grenade = Instantiate(grenadePrefab, unit.GetWorldPosition(), Quaternion.identity);
		var grenadeProjectile = grenade.GetComponent<GrenadeProjectile>();

		grenadeProjectile.Setup(gridPosition, OnGrenadeBehaviorComplete);

		ActionStart(onActionComplete);
	}

	private void OnGrenadeBehaviorComplete()
	{
		ActionComplete();
	}

	public override string GetActionName()
	{
		return "Grenade";
	}

	public override EnemyAIAction GetEnemyAIAction(GridPosition gridPosition)
	{
		return new EnemyAIAction
		{
			gridPosition = gridPosition,
			actionScore = 2
		};
	}

	public override List<GridPosition> GetValidActionGridPositions()
	{
		return GetValidActionGridPositions(unit.GetGridPosition());
	}

	public List<GridPosition> GetValidActionGridPositions(GridPosition gridPosition)
	{
		var validGridPositions = new List<GridPosition>();
		for (int x = -maxShootDistance; x <= maxShootDistance; x++)
		{
			for (int z = -maxShootDistance; z <= maxShootDistance; z++)
			{
				// circular area
				if (x * x + z * z > maxShootDistance * maxShootDistance)
				{
					continue;
				}

				var testGridPosition = new GridPosition(gridPosition.x + x, gridPosition.z + z);

				if ((gridPosition != testGridPosition)
					&& LevelGrid.Instance.IsValidGridPosition(testGridPosition)
					&& HasClearLOS(gridPosition, testGridPosition)
					)
				{
					validGridPositions.Add(testGridPosition);
				}
			}
		}
		return validGridPositions;
	}

	private bool HasClearLOS(GridPosition gridPosition, GridPosition testGridPosition)
	{
		var shooterPosition = LevelGrid.Instance.GetWorldPosition(gridPosition);
		var targetPosition = LevelGrid.Instance.GetWorldPosition(testGridPosition);
		var unitShoulderHeight = 1.7f;

		return !Physics.Linecast(shooterPosition + Vector3.up * unitShoulderHeight,
								targetPosition + Vector3.up * unitShoulderHeight,
								obstacleLayerMask);
	}
}
