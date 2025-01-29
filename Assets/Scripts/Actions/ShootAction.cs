using System;
using System.Collections.Generic;
using UnityEngine;

public class ShootAction : BaseAction
{
	public static event EventHandler<OnShootEventArgs> OnAnyShoot;
	public event EventHandler<OnShootEventArgs> OnShoot;
	public class OnShootEventArgs : EventArgs
	{
		public Unit shootingUnit;
		public Unit targetUnit;
	}

	private enum State
	{
		Aiming,
		Shooting,
		CoolingDown,
	}

	private State state;

	[SerializeField] private float aimDuration = 0.5f;
	[SerializeField] private float shootDuration = 1.5f;
	[SerializeField] private float cooldownDuration = 0.25f;

	[SerializeField] private LayerMask obstacleLayerMask;

	private float stateTimer;

	private int maxShootDistance = 7;

	private Unit targetUnit;

	public override string GetActionName()
	{
		return "Shoot";
	}

	private void Update()
	{
		if(!isActive)
		{
			return;
		}

		stateTimer -= Time.deltaTime;
		if(stateTimer <= 0)
		{
			NextState();
		}

		switch (state)
		{
			case State.Aiming:
				// aim at target
				if (targetUnit != null)
				{
					var aimDirection = targetUnit.transform.position - transform.position;
					transform.forward = Vector3.Lerp(transform.forward, aimDirection.normalized, Time.deltaTime * 5f);
				}
				break;
			case State.Shooting:
				// shoot at target
				break;
			case State.CoolingDown:
				// cooldown
				break;
		}
	}

	private void NextState()
	{
		switch (state)
		{
			case State.Aiming:
				// make sure we are aiming directly at target
				if (targetUnit != null)
				{
					var aimDirection = targetUnit.transform.position - transform.position;
					transform.forward = aimDirection.normalized;
				}

				state = State.Shooting;
				stateTimer = shootDuration;

				OnShoot?.Invoke(this, new OnShootEventArgs
				{
					shootingUnit = unit,
					targetUnit = targetUnit,
				});

				OnAnyShoot?.Invoke(this, new OnShootEventArgs
				{
					shootingUnit = unit,
					targetUnit = targetUnit,
				});

				targetUnit?.Damage(40);
				break;
			case State.Shooting:
				state = State.CoolingDown;
				stateTimer = cooldownDuration;
				break;
			case State.CoolingDown:
				ActionComplete();
				break;
		}
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

	public override void Act(GridPosition gridPosition, Action onActionComplete)
	{
		var unitList = LevelGrid.Instance.GetUnitListFromGridPosition(gridPosition);
		targetUnit = GetValidTarget(unitList);

		if (targetUnit == null)
		{
			return;
		}

		state = State.Aiming;
		stateTimer = aimDuration;

		ActionStart(onActionComplete);
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
					&& LevelGrid.Instance.IsOccupiedGridPosition(testGridPosition))
				{
					var unitList = LevelGrid.Instance.GetUnitListFromGridPosition(testGridPosition);
					foreach (var targetUnit in unitList)
					{
						if (targetUnit.IsEnemy() != this.unit.IsEnemy())
						{
							if(HasClearLOS(gridPosition, targetUnit))
							{
								validGridPositions.Add(testGridPosition);
								break;
							}
						}
					}
				}
			}
		}
		return validGridPositions;
	}

	private bool HasClearLOS(GridPosition gridPosition, Unit target)
	{
		var shooterPosition = LevelGrid.Instance.GetWorldPosition(gridPosition);
		var targetPosition = target.GetWorldPosition();
		var unitShoulderHeight = 1.7f;

		return !Physics.Linecast(shooterPosition + Vector3.up * unitShoulderHeight,
								targetPosition + Vector3.up * unitShoulderHeight,
								obstacleLayerMask);
	}

	public Unit GetTargetUnit() => targetUnit;

	public int GetMaxShootDistance() => maxShootDistance;

	public override EnemyAIAction GetEnemyAIAction(GridPosition gridPosition)
	{
		return new EnemyAIAction
		{
			actionScore = 100,
			gridPosition = gridPosition
		};
	}

	public int GetTargetCount(GridPosition gridPosition) => GetValidActionGridPositions(gridPosition).Count;

}
