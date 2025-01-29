using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class UnitWorldUI : MonoBehaviour
{
	[SerializeField] private TextMeshProUGUI actionPointsText;
	[SerializeField] private Unit unit;
	[SerializeField] private Image healthBarImage;
	[SerializeField] private HealthSystem healthSystem;

	private void Start()
	{
		Unit.OnAnyActionPointsChanged += Unit_OnAnyActionPointsChanged;
		UpdateActionPointsText();

		healthSystem.OnDamage += HealthSystem_OnDamage;
		UpdateHealthBar();
	}

	private void HealthSystem_OnDamage(object sender, EventArgs e)
	{
		UpdateHealthBar();
	}

	private void Unit_OnAnyActionPointsChanged(object sender, EventArgs e)
	{
		UpdateActionPointsText();
	}

	private void UpdateActionPointsText()
	{
		actionPointsText.text = unit.GetActionPoints().ToString();
	}

	private void UpdateHealthBar()
	{
		healthBarImage.fillAmount = healthSystem.GetHealthNormalized();
	}
}

