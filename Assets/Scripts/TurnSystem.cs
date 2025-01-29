using System;
using System.Collections.Generic;
using UnityEngine;

public class TurnSystem : MonoBehaviour
{
	public static TurnSystem Instance { get; private set; }

	public event EventHandler OnTurnChanged;

	public int TurnNumber { get; private set; }
	private bool isPlayerTurn = true;

	private void Awake()
	{
		if (Instance != null)
		{
			Debug.LogError("There should only be one TurnSystem in the scene");
			Destroy(gameObject);
			return;
		}
		Instance = this;

		TurnNumber = 1;
	}

	public void NextTurn()
	{
		if (!isPlayerTurn)
		{
			TurnNumber++;
		}
		isPlayerTurn = !isPlayerTurn;

		OnTurnChanged?.Invoke(this, EventArgs.Empty);
	}

	public bool IsPlayerTurn()
	{
		return isPlayerTurn;
	}
}
