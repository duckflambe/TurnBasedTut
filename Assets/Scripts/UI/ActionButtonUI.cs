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

	[SerializeField]
	private Image selectedImage;

	private BaseAction baseAction;

	private void Start()
	{
	}

	public void UpdateSelectedVisual()
	{
		SetSelectedVisual(baseAction == UnitActionSystem.Instance.GetSelectedAction());
	}

	public void SetSelectedVisual(bool isSelected)
	{
		selectedImage.enabled = isSelected;
	}

	public void SetBaseAction(BaseAction baseAction)
	{
		this.baseAction = baseAction;
		nameText.text = baseAction.GetActionName().ToUpper();

		button.onClick.AddListener(() =>
		{
			UnitActionSystem.Instance.SetSelectedAction(baseAction);
		});
	}
}
