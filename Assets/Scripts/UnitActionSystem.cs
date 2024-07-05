using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UnitActionSystem : MonoBehaviour
{
	public static UnitActionSystem Instance { get; private set; }

	public event EventHandler OnUnitSelected;
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

		if(EventSystem.current.IsPointerOverGameObject())
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
		if (Input.GetMouseButtonDown(0)
			&& selectedAction != null
			&& !isBusy)
		{
			var mouseGridPosition = LevelGrid.Instance.GetGridPosition(MouseWorld.GetPosition());
			if(selectedAction.IsValidActionGridPosition(mouseGridPosition))
			{
				selectedAction.Act(mouseGridPosition, ClearBusy);
				SetBusy();
			}
		}
	}

	private bool TrySelectUnit()
	{
		if (Input.GetMouseButtonDown(0))
		{
			var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			if (Physics.Raycast(ray, out var hit, float.MaxValue, unitLayerMask))
			{
				Unit unit;
				if (hit.collider.TryGetComponent<Unit>(out unit))
				{
					if (unit != SelectedUnit)
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
		SetSelectedAction(SelectedUnit.GetMoveAction());
		OnUnitSelected?.Invoke(this, EventArgs.Empty);
	}

	public void SetSelectedAction(BaseAction action)
	{
		selectedAction = action;
	}

	private void SetBusy()
	{
		isBusy = true;
	}

	private void ClearBusy()
	{
		isBusy = false;
	}

	public BaseAction GetSelectedAction()
	{
		return selectedAction;
	}
}
