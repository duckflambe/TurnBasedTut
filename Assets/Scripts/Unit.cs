using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class Unit : MonoBehaviour
{
	public static event EventHandler OnAnyActionPointsChanged;
	public static event EventHandler OnAnyUnitSpawned;
	public static event EventHandler OnAnyUnitDestroyed;

	[SerializeField] private bool isEnemy = false;
	[SerializeField] private int actionResetValue = 2;

	private GridPosition gridPosition;
	private HealthSystem healthSystem;
	private MoveAction moveAction;
	private SpinAction spinAction;
	private ShootAction shootAction;
	private BaseAction[] baseActions;

	private int actionPoints;
	
	private void Awake()
	{
		healthSystem = GetComponent<HealthSystem>();
		moveAction = GetComponent<MoveAction>();
		spinAction = GetComponent<SpinAction>();
		shootAction = GetComponent<ShootAction>();

		baseActions = GetComponents<BaseAction>();
		actionPoints = actionResetValue;
	}

	private void Start()
	{
		gridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
		LevelGrid.Instance.AddUnitToGridPosition(this, gridPosition);

		TurnSystem.Instance.OnTurnChanged += TurnSystem_OnTurnChanged;

		healthSystem.OnDeath += HealthSystem_OnDeath;

		OnAnyUnitSpawned?.Invoke(this, EventArgs.Empty);
	}

	private void HealthSystem_OnDeath(object sender, EventArgs e)
	{
		LevelGrid.Instance.RemoveUnitFromGridPosition(this, gridPosition);
		Destroy(gameObject);
		OnAnyUnitDestroyed?.Invoke(this, EventArgs.Empty);
	}

	void Update()
	{
		GridPosition nextGridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
		if(nextGridPosition != gridPosition)
		{
			var oldGridPosition = gridPosition;
			gridPosition = nextGridPosition;
			LevelGrid.Instance.MoveUnit(this, oldGridPosition, gridPosition);
		}
	}

	public GridPosition GetGridPosition()
	{
		return gridPosition;
	}

	public Vector3 GetWorldPosition()
	{
		return transform.position;
	}

	public float GetHeight()
	{
		if (TryGetComponent<Collider>(out var collider))
		{
			return collider.bounds.size.y;
		}

		return 0f;
	}

	public BaseAction[] GetBaseActions()
	{
		return baseActions;
	}

	public T GetAction<T>() where T : BaseAction
	{
		foreach (var action in baseActions)
		{
			if (action is T)
			{
				return action as T;
			}
		}

		return null;
	}

	public bool CanSpendActionPoints(BaseAction action)
	{
		return actionPoints >= action.GetActionPointCost();
	}

	public bool TrySpendActionPoints(BaseAction action)
	{
		if (!CanSpendActionPoints(action))
		{
			return false;
		}

		actionPoints -= action.GetActionPointCost();
		OnAnyActionPointsChanged?.Invoke(this, EventArgs.Empty);
		return true;
	}
	public int GetActionPoints()
	{
		return actionPoints;
	}

	private void TurnSystem_OnTurnChanged(object sender, System.EventArgs e)
	{
		if((isEnemy && TurnSystem.Instance.IsPlayerTurn())
			|| (!isEnemy && !TurnSystem.Instance.IsPlayerTurn()))
		{
			return;
		}

		actionPoints = actionResetValue;
		OnAnyActionPointsChanged?.Invoke(this, EventArgs.Empty);
	}

	public bool IsEnemy() => isEnemy;

	public void Damage(int damage)
	{
		healthSystem?.TakeDamage(damage);
	}
}
