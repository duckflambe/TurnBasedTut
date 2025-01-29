using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class BaseAction : MonoBehaviour
{
	public static event EventHandler OnAnyActionStarted;
	public static event EventHandler OnAnyActionCompleted;

	protected Unit unit;
	protected bool isActive = false;

	protected Action onComplete;

	protected virtual void Awake()
	{
		unit = GetComponent<Unit>();
	}

	public abstract string GetActionName();

	public abstract void Act(GridPosition gridPosition, Action onActionComplete);

	public abstract List<GridPosition> GetValidActionGridPositions();

	public bool IsValidActionGridPosition(GridPosition gridPosition)
	{
		var validGridPositions = GetValidActionGridPositions();
		return validGridPositions.Contains(gridPosition);
	}

	public virtual int GetActionPointCost()
	{
		return 1;
	}

	protected void ActionStart(Action onActionComplete)
	{
		isActive = true;
		onComplete = onActionComplete;

		OnAnyActionStarted?.Invoke(this, EventArgs.Empty);
	}

	protected void ActionComplete()
	{
		isActive = false;
		onComplete?.Invoke();

		OnAnyActionCompleted?.Invoke(this, EventArgs.Empty);
	}

	public Unit GetUnit() => unit;

	public EnemyAIAction GetBestEnemyAIAction()
	{
		var enemyAIAction = new List<EnemyAIAction>();
		var validActionGridPositions = GetValidActionGridPositions();

		foreach (var gridPosition in validActionGridPositions)
		{
			var enemyAIActionAtGridPosition = GetEnemyAIAction(gridPosition);
			enemyAIAction.Add(enemyAIActionAtGridPosition);
		}

		if (enemyAIAction.Count == 0)
		{
			return null;
		}

		enemyAIAction.Sort((EnemyAIAction a, EnemyAIAction b) => b.actionScore - a.actionScore);

		return enemyAIAction[0];
	}

	public abstract EnemyAIAction GetEnemyAIAction(GridPosition gridPosition);
}
