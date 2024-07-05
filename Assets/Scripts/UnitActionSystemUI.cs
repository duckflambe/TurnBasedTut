using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitActionSystemUI : MonoBehaviour
{
	[SerializeField]
	private Transform actionButtonPrefab;

	[SerializeField]
	private Transform actionButtonContainerTransform;

	private void Start()
	{
		UnitActionSystem.Instance.OnUnitSelected += UnitActionSystem_OnSelectedUnitChanged;
		CreateUnitActionButtons();
	}

	private void CreateUnitActionButtons()
	{
		foreach (Transform child in actionButtonContainerTransform)
		{
			Destroy(child.gameObject);
		}

		var unit = UnitActionSystem.Instance.SelectedUnit;
		var baseActions = unit.GetBaseActions();
		foreach (var baseAction in baseActions)
		{
			var buttonTrans = Instantiate(actionButtonPrefab, actionButtonContainerTransform);
			var button = buttonTrans.GetComponent<ActionButtonUI>();
			button.SetBaseAction(baseAction);
		}
	}

	private void UnitActionSystem_OnSelectedUnitChanged(object sender, System.EventArgs e)
	{
		CreateUnitActionButtons();
	}
}
