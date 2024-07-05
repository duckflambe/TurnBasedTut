using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridSystemVisual : MonoBehaviour
{
	public static GridSystemVisual Instance { get; private set; }

	[SerializeField]
	private Transform gridVisualPrefab;

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
	}

	private void Update()
	{
		UpdateGridVisual();
	}

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

	public void ShowGridPositions(List<GridPosition> gridPositions)
	{
		HideAllGridPositions();
		foreach (var gridPosition in gridPositions)
		{
			gridSystemVisualSingles[gridPosition.x, gridPosition.z].Show();
		}
	}

	private void UpdateGridVisual()
	{
		var selectedAction = UnitActionSystem.Instance.GetSelectedAction();
		ShowGridPositions(selectedAction.GetValidActionGridPositions());
	}
}
