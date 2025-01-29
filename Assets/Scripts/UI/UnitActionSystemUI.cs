using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


public class UnitActionSystemUI : MonoBehaviour
{
	[SerializeField]
	private Transform actionButtonPrefab;

	[SerializeField]
	private Transform actionButtonContainerTransform;

	[SerializeField]
	private TextMeshProUGUI actionPointsText;

	private List<ActionButtonUI> actionButtons = new();

	private void Start()
	{
		UnitActionSystem.Instance.OnUnitSelected += UnitActionSystem_OnSelectedUnitChanged;
		UnitActionSystem.Instance.OnActionSelected += UnitActionSystem_OnSelectedActionChanged;
		UnitActionSystem.Instance.OnActionPointsChanged += UnitActionSystem_OnActionPointsChanged;
		TurnSystem.Instance.OnTurnChanged += TurnSystem_OnTurnChanged;
		Unit.OnAnyActionPointsChanged += Unit_OnAnyActionPointsChanged;

		CreateUnitActionButtons();
		UpdateActionPoints();
	}

	private void TurnSystem_OnTurnChanged(object sender, EventArgs e)
	{
		UpdateActionPoints();
	}

	private void Unit_OnAnyActionPointsChanged(object sender, EventArgs e)
	{
		UpdateActionPoints(); 
	}

	private void CreateUnitActionButtons()
	{
		foreach (Transform child in actionButtonContainerTransform)
		{
			Destroy(child.gameObject);
		}

		actionButtons.Clear();

		var unit = UnitActionSystem.Instance.SelectedUnit;
		var baseActions = unit.GetBaseActions();
		foreach (var baseAction in baseActions)
		{
			var buttonTrans = Instantiate(actionButtonPrefab, actionButtonContainerTransform);

			var button = buttonTrans.GetComponent<ActionButtonUI>();
			button.SetBaseAction(baseAction);
			actionButtons.Add(button);
		}

		UpdateSelectedVisual();
	}

	private void UnitActionSystem_OnSelectedUnitChanged(object sender, System.EventArgs e)
	{
		CreateUnitActionButtons();
	}

	private void UnitActionSystem_OnSelectedActionChanged(object sender, EventArgs e)
	{
		UpdateSelectedVisual();
	}

	private void UnitActionSystem_OnActionPointsChanged(object sender, EventArgs e)
	{
		UpdateActionPoints();
	}

	private void UpdateSelectedVisual()
	{
		foreach (var button in actionButtons)
		{
			button.UpdateSelectedVisual();
		}
	}

	private void UpdateActionPoints()
	{
		var unit = UnitActionSystem.Instance.SelectedUnit;
		actionPointsText.text = $"Action Points: {unit.GetActionPoints()}";
	}
}
