using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridSystem<TGridObject>
{
	private int width;
	private int height;
	private float cellSize;

	private TGridObject[,] gridObjects;


	public GridSystem(int width, int height, float cellSize,
		Func<GridSystem<TGridObject>, GridPosition, TGridObject> createGridObject)
	{
		this.width = width;
		this.height = height;
		this.cellSize = cellSize;

		gridObjects = new TGridObject[width, height];
		for (int x = 0; x < width; x++)
		{
			for (int z = 0; z < height; z++)
			{
				gridObjects[x, z] = createGridObject(this, new GridPosition(x, z));
			}
		}

		this.cellSize = cellSize;
	}

	public Vector3 GetWorldPosition(int x, int z)
	{
		return new Vector3(x, 0, z) * cellSize;
	}

	public Vector3 GetWorldPosition(GridPosition gridPosition)
	{
		return GetWorldPosition(gridPosition.x, gridPosition.z);
	}

	public GridPosition GetGridPosition(Vector3 worldPosition)
	{
		return new GridPosition(Mathf.RoundToInt(worldPosition.x / cellSize),
								Mathf.RoundToInt(worldPosition.z / cellSize));
	}

	public void CreateDebugObjects(Transform debugPrefab)
	{
		for (int x = 0; x < width; x++)
		{
			for (int z = 0; z < height; z++)
			{
				Transform debugTransform = GameObject.Instantiate(debugPrefab, GetWorldPosition(x, z), Quaternion.identity);
				debugTransform.GetComponent<GridDebugObject>().SetGridObject(GetGridObject(x,z));
			}
		}
	}

	public TGridObject GetGridObject(int x, int z)
	{
		if (x >= 0 && z >= 0 && x < width && z < height)
		{
			return gridObjects[x, z];
		}
		return default;
	}

	public TGridObject GetGridObject(GridPosition gridPosition)
	{
		return GetGridObject(gridPosition.x, gridPosition.z);
	}

	public bool IsValidPosition(int x, int z)
	{
		return x >= 0 && z >= 0 && x < width && z < height;
	}

	public bool IsValidPosition(GridPosition gridPosition)
	{
		return IsValidPosition(gridPosition.x, gridPosition.z);
	}

	public int GetWidth()
	{
		return width;
	}

	public int GetHeight()
	{
		return height;
	}
}
