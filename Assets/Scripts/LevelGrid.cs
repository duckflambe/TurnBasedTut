using System;
using System.Collections.Generic;
using UnityEngine;

public class LevelGrid : MonoBehaviour
{
	public static LevelGrid Instance { get; private set; }

	public event EventHandler OnGridObjectChanged;

	[SerializeField] private int width = 20;
	[SerializeField] private int height = 20;
	[SerializeField] private float cellSize = 2f;

	[SerializeField] private Transform debugPrefab;

	private GridSystem<GridObject> gridSystem;

	private void Awake()
	{
		if (Instance != null)
		{
			Debug.LogError("There should only be one LevelGrid in the scene");
			Destroy(gameObject);
			return;
		}
		Instance = this;

		gridSystem = new GridSystem<GridObject>(width, height, cellSize,
			(GridSystem<GridObject> g, GridPosition p) => new GridObject(g,p));
		//gridSystem.CreateDebugObjects(debugPrefab);
	}

	private void Update()
	{
	}

	public int Width => width;
	public int Height => height;
	public float CellSize => cellSize;


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

		OnGridObjectChanged?.Invoke(this, EventArgs.Empty);
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

	public IInteractable GetInteractableAtGridPosition(GridPosition gridPosition)
	{
		var gridObject = gridSystem.GetGridObject(gridPosition);
		return gridObject.Interactable;
	}

	public void SetInteractableAtGridPosition(IInteractable interactable, GridPosition gridPosition)
	{
		var gridObject = gridSystem.GetGridObject(gridPosition);
		gridObject.Interactable = interactable;
	}
}
