using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TurnSystemUI : MonoBehaviour
{
	[SerializeField]
	private TextMeshProUGUI turnNumberText;

	[SerializeField]
	private Button nextTurnButton;

	[SerializeField] private GameObject enemyTurnMessage;

	private void Start()
	{
		TurnSystem.Instance.OnTurnChanged += TurnSystem_OnTurnChanged;
		nextTurnButton.onClick.AddListener(() =>
		{
			TurnSystem.Instance.NextTurn();
		});

		UpdateTurnNumberText();
		UpdateEnemyTurnMessage();
		UpdateEndTurnButton();
	}

	private void TurnSystem_OnTurnChanged(object sender, System.EventArgs e)
	{
		UpdateTurnNumberText();
		UpdateEnemyTurnMessage();
		UpdateEndTurnButton();
	}

	private void UpdateTurnNumberText()
	{
		turnNumberText.text = "TURN: " + TurnSystem.Instance.TurnNumber;
	}

	private void UpdateEnemyTurnMessage()
	{
		enemyTurnMessage.SetActive(!TurnSystem.Instance.IsPlayerTurn());
	}

	private void UpdateEndTurnButton()
	{
		nextTurnButton.gameObject.SetActive(TurnSystem.Instance.IsPlayerTurn());
	}
}
