using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseAction : MonoBehaviour
{
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
}
