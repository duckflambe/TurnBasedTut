using System;
using System.Collections.Generic;
using UnityEngine;

public class SwordAction : BaseAction
{
	public event EventHandler OnActionStarted;
	public event EventHandler OnActionCompleted;

	private enum State
	{
		PreHit,
		PostHit,
	}
	private State state;

	[SerializeField] private int damage = 200;
	[SerializeField] private float preHit = 0.7f;
	[SerializeField] private float postHit = 0.5f;
	private float stateTimer = 0f;

	private Unit targetUnit;

	private void Update()
	{
		if (!isActive)
		{
			return;
		}

		stateTimer -= Time.deltaTime;
		if (stateTimer <= 0)
		{
			NextState();
		}

		switch (state)
		{
			case State.PreHit:
				// aim at target
				if (targetUnit != null)
				{
					var aimDirection = targetUnit.transform.position - transform.position;
					transform.rotation = Quaternion.LookRotation(aimDirection, Vector3.up);
				}
				break;
			case State.PostHit:
				break;
		}
	}

	private void NextState()
	{
		switch (state)
		{
			case State.PreHit:
				state = State.PostHit;
				stateTimer = postHit;
				targetUnit?.Damage(damage);
				break;
			case State.PostHit:
				OnActionCompleted?.Invoke(this, EventArgs.Empty);
				ActionComplete();
				break;
		}
	}

	public override void Act(GridPosition gridPosition, Action onActionComplete)
	{
		targetUnit = GetValidTarget(LevelGrid.Instance.GetUnitListFromGridPosition(gridPosition));
		if(null == targetUnit)
		{
			Debug.LogError("No valid target");
			return;
		}

		state = State.PreHit;
		stateTimer = preHit;

		OnActionStarted?.Invoke(this, EventArgs.Empty);
		ActionStart(onActionComplete);
	}

	public override string GetActionName()
	{
		return "Sword";
	}

	public int GetMaxShootDistance() => 1;

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
					)
				{
					var unitList = LevelGrid.Instance.GetUnitListFromGridPosition(testGridPosition);
					if(null != GetValidTarget(unitList))
					{
						validGridPositions.Add(testGridPosition);
					}
				}
			}
		}
		return validGridPositions;
	}

	private Unit GetValidTarget(List<Unit> targets)
	{
		foreach (var target in targets)
		{
			if (target.IsEnemy() != unit.IsEnemy())
			{
				return target;
			}
		}

		return null;
	}

}
