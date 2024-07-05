using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ActionButtonUI : MonoBehaviour
{
	[SerializeField]
	private TextMeshProUGUI nameText;

	[SerializeField]
	private Button button;

	public void SetBaseAction(BaseAction baseAction)
	{
		nameText.text = baseAction.GetActionName().ToUpper();

		button.onClick.AddListener(() =>
		{
			UnitActionSystem.Instance.SetSelectedAction(baseAction);
		});
	}
}
