using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;

public class UnitManager : MonoBehaviour
{
	public static UnitManager Instance { get; private set; }

	private List<Unit> units = new List<Unit>();
	private List<Unit> friendlyUnits = new List<Unit>();
	private List<Unit> enemyUnits = new List<Unit>();

	public List<Unit> GetUnits() => units;
	public List<Unit> GetFriendlyUnits() => friendlyUnits;
	public List<Unit> GetEnemyUnits() => enemyUnits;

	private void Awake()
	{
		if (Instance != null)
		{
			Debug.LogError("There should never be two UnitManager.");
			Destroy(gameObject);
			return;
		}
		Instance = this;
	}

	private void Start()
	{
		Unit.OnAnyUnitSpawned += Unit_OnAnyUnitSpawned;
		Unit.OnAnyUnitDestroyed += Unit_OnAnyUnitDestroyed;
	}

	private void Unit_OnAnyUnitSpawned(object sender, EventArgs e)
	{
		Unit unit = (Unit)sender;
		units.Add(unit);
		if (unit.IsEnemy())
		{
			enemyUnits.Add(unit);
		}
		else
		{
			friendlyUnits.Add(unit);
		}
	}

	private void Unit_OnAnyUnitDestroyed(object sender, EventArgs e)
	{
		Unit unit = (Unit)sender;
		units.Remove(unit);
		if (unit.IsEnemy())
		{
			enemyUnits.Remove(unit);
		}
		else
		{
			friendlyUnits.Remove(unit);
		}
	}

}
