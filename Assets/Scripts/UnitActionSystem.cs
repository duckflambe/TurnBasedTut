using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UnitActionSystem : MonoBehaviour
{
	public static UnitActionSystem Instance { get; private set; }

	public event EventHandler OnUnitSelected;
	public event EventHandler OnActionSelected;
	public event EventHandler<bool> OnBusyChanged;

	public event EventHandler OnActionPointsChanged;
	private bool isBusy = false;

	[SerializeField]
	private LayerMask unitLayerMask;

	[SerializeField]
	private Unit selectedUnit;
	public Unit SelectedUnit { get => selectedUnit; private set => selectedUnit = value; }

	private BaseAction selectedAction;

	private void Awake()
	{
		if (Instance != null)
		{
			Debug.LogError("There should only be one UnitActionSystem in the scene");
			Destroy(gameObject);
			return;
		}
		Instance = this;
	}

	private void Start()
	{
		SetSelectedUnit(selectedUnit);
	}

	private void Update()
	{
		if (isBusy)
		{
			return;
		}

		if (!TurnSystem.Instance.IsPlayerTurn())
		{
			return;
		}

		if ((EventSystem.current != null)
			&& EventSystem.current.IsPointerOverGameObject())
		{
			return;
		}

		if (TrySelectUnit())
		{
			return;
		}

		HandleSelectedAction();
	}

	private void HandleSelectedAction()
	{
		if (InputManager.Instance.IsMouseLeftClicked()
			&& selectedAction != null
			&& !isBusy)
		{
			var mouseGridPosition = LevelGrid.Instance.GetGridPosition(MouseWorld.GetPosition());
			if (selectedAction.IsValidActionGridPosition(mouseGridPosition))
			{
				if (SelectedUnit.TrySpendActionPoints(selectedAction))
				{
					selectedAction.Act(mouseGridPosition, ClearBusy);
					OnActionPointsChanged?.Invoke(this, EventArgs.Empty);
					SetBusy();
				}
			}
		}
	}

	private bool TrySelectUnit()
	{
		if (InputManager.Instance.IsMouseLeftClicked())
		{
			var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			if (Physics.Raycast(ray, out var hit, float.MaxValue, unitLayerMask))
			{
				Unit unit;
				if (hit.collider.TryGetComponent<Unit>(out unit))
				{
					if ((unit != SelectedUnit)
						&& !unit.IsEnemy())
					{
						SetSelectedUnit(unit);
						return true;
					}
				}
			}
		}

		return false;
	}

	private void SetSelectedUnit(Unit unit)
	{
		SelectedUnit = unit;
		SetSelectedAction(SelectedUnit.GetAction<MoveAction>());
		OnUnitSelected?.Invoke(this, EventArgs.Empty);
		OnActionPointsChanged?.Invoke(this, EventArgs.Empty);
	}

	public void SetSelectedAction(BaseAction action)
	{
		selectedAction = action;
		OnActionSelected?.Invoke(this, EventArgs.Empty);
	}

	public bool IsSelectedActionCircularRange 
		=> (selectedAction != null) 
		&& ((selectedAction is ShootAction)
			|| (selectedAction is GrenadeAction));

	private void SetBusy()
	{
		isBusy = true;
		OnBusyChanged?.Invoke(this, isBusy);
	}

	private void ClearBusy()
	{
		isBusy = false;
		OnBusyChanged?.Invoke(this, isBusy);
	}

	public BaseAction GetSelectedAction()
	{
		return selectedAction;
	}
}
