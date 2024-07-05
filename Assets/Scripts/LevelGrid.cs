using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGrid : MonoBehaviour
{
	public static LevelGrid Instance { get; private set; }

	[SerializeField] private Transform debugPrefab;

	private GridSystem gridSystem;

	private void Awake()
	{
		if (Instance != null)
		{
			Debug.LogError("There should only be one LevelGrid in the scene");
			Destroy(gameObject);
			return;
		}
		Instance = this;

		gridSystem = new GridSystem(10, 10, 2f);
		gridSystem.CreateDebugObjects(debugPrefab);
	}

	private void Update()
	{
	}

	public void AddUnitToGridPosition(Unit unit, GridPosition position)
	{
		var gridObject = gridSystem.GetGridObject(position);
		gridObject.AddUnit(unit);
	}

	public void RemoveUnitFromGridPosition(Unit unit, GridPosition position)
	{
		var gridObject = gridSystem.GetGridObject(position);
		gridObject.RemoveUnit(unit);
	}

	public List<Unit> GetUnitListFromGridPosition(GridPosition position)
	{
		var gridObject = gridSystem.GetGridObject(position);
		return gridObject.GetUnitList();
	}

	public void MoveUnit(Unit unit, GridPosition from, GridPosition to)
	{
		RemoveUnitFromGridPosition(unit, from);
		AddUnitToGridPosition(unit, to);
	}

	public GridPosition GetGridPosition(Vector3 worldPosition) => gridSystem.GetGridPosition(worldPosition);

	public Vector3 GetWorldPosition(GridPosition gridPosition) => gridSystem.GetWorldPosition(gridPosition);

	public bool IsValidGridPosition(GridPosition gridPosition) => gridSystem.IsValidPosition(gridPosition);

	public int GetWidth() => gridSystem.GetWidth();
	public int GetHeight() => gridSystem.GetHeight();

	public bool IsOccupiedGridPosition(GridPosition gridPosition)
	{
		var gridObject = gridSystem.GetGridObject(gridPosition);
		return gridObject.GetUnitList().Count > 0;
	}
}
