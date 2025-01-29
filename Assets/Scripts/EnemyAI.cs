using System;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
	private enum State
	{
		WaitingForEnemyTurn,
		TakingTurn,
		Busy
	}

	private State state;

	private float timer = 0.5f;

	private void Awake()
	{
		state = State.WaitingForEnemyTurn;
	}

	private void Start()
	{
		TurnSystem.Instance.OnTurnChanged += TurnSystem_OnTurnChanged;
	}

	private void Update()
	{
		if (TurnSystem.Instance.IsPlayerTurn())
		{
			return;
		}

		switch(state)
		{
			case State.WaitingForEnemyTurn:
				break;
			case State.TakingTurn:
				timer -= Time.deltaTime;
				if (timer <= 0)
				{
					if (TryEnemyAIAction(SetStateTakingTurn))
					{
						state = State.Busy;
					}
					else
					{
						TurnSystem.Instance.NextTurn();
					}
				}
				break;
			case State.Busy:
				break;
		}
	}

	private bool TryEnemyAIAction(Action onEnemyAIActionComplete)
	{
		foreach(var enemyUnit in UnitManager.Instance.GetEnemyUnits())
		{
			EnemyAIAction bestEnemyAIAction = null;
			BaseAction bestBaseAction = null;

			if (enemyUnit.GetActionPoints() <= 0)
			{
				continue;
			}

			foreach (var baseAction in enemyUnit.GetBaseActions())
			{
				if (baseAction.GetActionPointCost() > enemyUnit.GetActionPoints())
				{
					continue;
				}

				var currentBestEnemyAIAction = baseAction.GetBestEnemyAIAction();
				if (currentBestEnemyAIAction != null)
				{
					if ((bestEnemyAIAction == null)
						|| (currentBestEnemyAIAction.actionScore > bestEnemyAIAction.actionScore))
					{
						bestEnemyAIAction = currentBestEnemyAIAction;
						bestBaseAction = baseAction;
					}
				}
			}

			if(bestBaseAction == null)
			{
				continue;
			}

			if (enemyUnit.TrySpendActionPoints(bestBaseAction))
			{
				bestBaseAction.Act(bestEnemyAIAction.gridPosition, onEnemyAIActionComplete);
				return true;
			}

		}

		return false;
	}

	private void SetStateTakingTurn()
	{
		state = State.TakingTurn;
		timer = 0.5f;
	}

	private void TurnSystem_OnTurnChanged(object sender, EventArgs e)
	{
		if (TurnSystem.Instance.IsPlayerTurn())
		{
			state = State.WaitingForEnemyTurn;
		}
		else
		{
			state = State.TakingTurn;
			timer = 2f;
		}
	}
}
