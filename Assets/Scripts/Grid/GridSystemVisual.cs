using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridSystemVisual : MonoBehaviour
{
	public static GridSystemVisual Instance { get; private set; }

	public enum GridVisualType
	{
		White,
		Red,
		RedTransparent,
		Yellow,
		Blue
	}

	[Serializable]
	public struct GridVisualTypeMaterial
	{
		public GridVisualType gridVisualType;
		public Material material;
	}

	[SerializeField] private Transform gridVisualPrefab;
	[SerializeField] private List<GridVisualTypeMaterial> gridVisualTypeMaterialList;

	private GridSystemVisualSingle[,] gridSystemVisualSingles;

	private void Awake()
	{
		if (Instance != null)
		{
			Debug.LogError("There should only be one GridSystemVisual in the scene");
			Destroy(gameObject);
			return;
		}
		Instance = this;
	}

	private void Start()
	{
		var levelGrid = LevelGrid.Instance;

		gridSystemVisualSingles = new GridSystemVisualSingle[levelGrid.GetWidth(), levelGrid.GetHeight()];

		for (int x = 0; x < levelGrid.GetWidth(); x++)
		{
			for (int z = 0; z < levelGrid.GetHeight(); z++)
			{
				var gridPosition = new GridPosition(x, z);
				Transform visualTransform = Instantiate(gridVisualPrefab, levelGrid.GetWorldPosition(gridPosition), Quaternion.identity);

				gridSystemVisualSingles[x, z] = visualTransform.GetComponent<GridSystemVisualSingle>();
			}
		}

		UnitActionSystem.Instance.OnActionSelected += UnitActionSystem_OnActionSelected;
		LevelGrid.Instance.OnGridObjectChanged += LevelGrid_OnGridObjectChanged;
		UpdateGridVisual();
	}

	private void LevelGrid_OnGridObjectChanged(object sender, EventArgs e)
	{
		UpdateGridVisual();
	}

	private void UnitActionSystem_OnActionSelected(object sender, EventArgs e)
	{
		UpdateGridVisual();
	}

	// don't update every frame
	//private void Update()
	//{
	//	UpdateGridVisual();
	//}

	public void HideAllGridPositions()
	{
		var levelGrid = LevelGrid.Instance;
		for (int x = 0; x < levelGrid.GetWidth(); x++)
		{
			for (int z = 0; z < levelGrid.GetHeight(); z++)
			{
				gridSystemVisualSingles[x, z].Hide();
			}
		}
	}

	public void ShowGridPositions(List<GridPosition> gridPositions, GridVisualType gridVisualType)
	{
		var material = GetGridVisualTypeMaterial(gridVisualType);

		foreach (var gridPosition in gridPositions)
		{
			gridSystemVisualSingles[gridPosition.x, gridPosition.z].Show(material);
		}
	}

	public void ShowGridPositionRange(GridPosition gridPosition, int range, GridVisualType gridVisualType)
	{
		List<GridPosition> gridPositions = new List<GridPosition>();
		bool useCircularRange = UnitActionSystem.Instance.IsSelectedActionCircularRange;

		for (int x = -range; x <= range; x++)
		{
			for (int z = -range; z <= range; z++)
			{
				// circular area
				if (useCircularRange
					&& (x * x + z * z > range * range))
				{
					continue;
				}
				var testGridPosition = new GridPosition(gridPosition.x + x, gridPosition.z + z);
				if (LevelGrid.Instance.IsValidGridPosition(testGridPosition))
				{
					gridPositions.Add(testGridPosition);
				}
			}
		}
		ShowGridPositions(gridPositions, gridVisualType);
	}

	private void UpdateGridVisual()
	{
		HideAllGridPositions();

		var selectedAction = UnitActionSystem.Instance.GetSelectedAction();

		var gridVisualType = GridVisualType.White;
		switch (selectedAction)
		{
			case ShootAction shootAction:
				gridVisualType = GridVisualType.Red;
				var unitGridPosition = UnitActionSystem.Instance.SelectedUnit.GetGridPosition();
				ShowGridPositionRange(unitGridPosition, shootAction.GetMaxShootDistance(), GridVisualType.RedTransparent);
				break;
			case SwordAction swordAction:
				gridVisualType = GridVisualType.Red;
				unitGridPosition = UnitActionSystem.Instance.SelectedUnit.GetGridPosition();
				ShowGridPositionRange(unitGridPosition, swordAction.GetMaxShootDistance(), GridVisualType.RedTransparent);
				break;
			case SpinAction _:
				gridVisualType = GridVisualType.Blue;
				break;
			case InteractAction _:
				gridVisualType = GridVisualType.Blue;
				break;
		}

		ShowGridPositions(selectedAction.GetValidActionGridPositions(), gridVisualType);
	}

	private Material GetGridVisualTypeMaterial(GridVisualType gridVisualType)
	{
		foreach (var gridVisualTypeMaterial in gridVisualTypeMaterialList)
		{
			if (gridVisualTypeMaterial.gridVisualType == gridVisualType)
			{
				return gridVisualTypeMaterial.material;
			}
		}

		Debug.LogError("GridVisualTypeMaterial not found for " + gridVisualType);
		return null;
	}
}
