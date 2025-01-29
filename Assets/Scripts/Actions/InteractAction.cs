using System;
using System.Collections.Generic;
using UnityEngine;

public class InteractAction : BaseAction
{
	public override void Act(GridPosition gridPosition, Action onActionComplete)
	{
		var interactable = LevelGrid.Instance.GetInteractableAtGridPosition(gridPosition);
		if (interactable != null) {
			ActionStart(onActionComplete);
			interactable.Interact(OnInteractComplete);
		}
	}

	private void OnInteractComplete()
	{
		ActionComplete();
	}

	public override string GetActionName()
	{
		return "Interact";
	}

	public override EnemyAIAction GetEnemyAIAction(GridPosition gridPosition)
	{
		return new EnemyAIAction
		{
			actionScore = 0,
			gridPosition = gridPosition,
		};
	}

	public override List<GridPosition> GetValidActionGridPositions()
	{
		var gridPosition = unit.GetGridPosition();
		var validGridPositions = new List<GridPosition>();
		for (int x = -1; x <= 1; x++)
		{
			for (int z = -1; z <= 1; z++)
			{
				var testGridPosition = new GridPosition(gridPosition.x + x, gridPosition.z + z);

				if ((gridPosition != testGridPosition)
					&& LevelGrid.Instance.IsValidGridPosition(testGridPosition)
					&& LevelGrid.Instance.GetInteractableAtGridPosition(testGridPosition) != null
					)
				{
					validGridPositions.Add(testGridPosition);
				}
			}
		}
		return validGridPositions;
	}

}
