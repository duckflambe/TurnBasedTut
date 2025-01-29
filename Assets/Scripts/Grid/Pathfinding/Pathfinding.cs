using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinding : MonoBehaviour
{
	public static Pathfinding Instance { get; private set; }

	[SerializeField] private LayerMask obstacleLayerMask;
	[SerializeField] private Transform gridDebugObjectPrefab;

	private GridSystem<PathNode> gridSystem;

	private const int MOVE_STRAIGHT_COST = 10;
	private const int MOVE_DIAGONAL_COST = 14;

	private void Awake()
	{
		if (Instance != null)
		{
			Debug.LogWarning("Another instance of Pathfinding already exists. Destroying this instance.");
			Destroy(gameObject);
			return;
		}
		Instance = this;
	}

	private void Start()
	{
		var levelGrid = LevelGrid.Instance;

		gridSystem = new GridSystem<PathNode>(levelGrid.Width, levelGrid.Height, levelGrid.CellSize,
			(grid, gridPosition) => new PathNode(gridPosition));

		SetupObstacles();

		//gridSystem.CreateDebugObjects(gridDebugObjectPrefab);
	}

	public void SetupObstacles()
	{
		var levelGrid = LevelGrid.Instance;
		for (int x = 0; x < levelGrid.Width; x++)
		{
			for (int z = 0; z < levelGrid.Height; z++)
			{
				var gridPosition = new GridPosition(x, z);
				var worldPosition = levelGrid.GetWorldPosition(gridPosition);
				const float raycastOffset = 5f;

				bool hitObstacle = (Physics.Raycast(worldPosition + Vector3.down * raycastOffset, 
									Vector3.up, raycastOffset * 2, obstacleLayerMask));

				var gridObject = gridSystem.GetGridObject(gridPosition);
				gridObject.IsWalkable = !hitObstacle;
			}
		}
	}

	public (List<GridPosition> path, int? length) FindPath(GridPosition start, GridPosition end)
	{
		var openList = new List<PathNode>();
		var closedList = new List<PathNode>();

		ResetGridSystem();

		var endNode = gridSystem.GetGridObject(end);
		var startNode = gridSystem.GetGridObject(start);
		openList.Add(startNode);

		startNode.GCost = 0;
		startNode.HCost = CalculateDistanceCost(start, end);

		while (openList.Count > 0)
		{
			var currentNode = GetLowestFCostNode(openList);
			if (currentNode == endNode)
			{
				// PATH FOUND
				return (CalculatePath(currentNode), endNode.FCost);
			}

			openList.Remove(currentNode);
			closedList.Add(currentNode);

			foreach (var neighbourNode in GetNeighbourList(currentNode))
			{
				if (!neighbourNode.IsWalkable
					|| closedList.Contains(neighbourNode)) 
					continue;

				int tentativeGCost = currentNode.GCost + CalculateDistanceCost(currentNode.GridPosition, neighbourNode.GridPosition);
				if (tentativeGCost < neighbourNode.GCost)
				{
					neighbourNode.CameFromNode = currentNode;
					neighbourNode.GCost = tentativeGCost;
					neighbourNode.HCost = CalculateDistanceCost(neighbourNode.GridPosition, end);
					if (!openList.Contains(neighbourNode))
					{
						openList.Add(neighbourNode);
					}
				}
			}
		}

		return (null, 0);
	}

	public bool HasPath(GridPosition start, GridPosition end, int length)
	{
		var (path, pathLength) = FindPath(start, end);
		return ((path != null) && (pathLength <= length * MOVE_STRAIGHT_COST));
	}

	public void ResetGridSystem()
	{
		var levelGrid = LevelGrid.Instance;
		for (int x = 0; x < levelGrid.Width; x++)
		{
			for (int z = 0; z < levelGrid.Height; z++)
			{
				var pathNode = gridSystem.GetGridObject(x, z);
				pathNode.GCost = int.MaxValue;
				pathNode.HCost = 0;
				pathNode.CameFromNode = null;
			}
		}
	}

	public int CalculateDistanceCost(GridPosition a, GridPosition b)
	{
		int xDistance = Mathf.Abs(a.x - b.x);
		int zDistance = Mathf.Abs(a.z - b.z);
		int remaining = Mathf.Abs(xDistance - zDistance);
		return MOVE_DIAGONAL_COST * Mathf.Min(xDistance, zDistance)
				+ MOVE_STRAIGHT_COST * remaining;
	}

	private PathNode GetLowestFCostNode(List<PathNode> pathNodes)
	{
		PathNode lowestFCostNode = pathNodes[0];
		for (int i = 1; i < pathNodes.Count; i++)
		{
			if (pathNodes[i].FCost < lowestFCostNode.FCost)
			{
				lowestFCostNode = pathNodes[i];
			}
		}
		return lowestFCostNode;
	}

	private List<PathNode> GetNeighbourList(PathNode currentNode)
	{
		var neighbourList = new List<PathNode>();
		var neighbourOffsets = new List<GridPosition>
		{
			new GridPosition(-1, 0),
			new GridPosition(1, 0),
			new GridPosition(0, -1),
			new GridPosition(0, 1),
			new GridPosition(-1, -1),
			new GridPosition(-1, 1),
			new GridPosition(1, -1),
			new GridPosition(1, 1)
		};

		var width = LevelGrid.Instance.Width;
		var height = LevelGrid.Instance.Height;

		foreach (var offset in neighbourOffsets)
		{
			var neighbourPosition = new GridPosition(currentNode.GridPosition.x + offset.x, currentNode.GridPosition.z + offset.z);
			if (neighbourPosition.x >= 0 && neighbourPosition.z >= 0 
				&& neighbourPosition.x < width && neighbourPosition.z < height)
			{
				neighbourList.Add(gridSystem.GetGridObject(neighbourPosition));
			}
		}

		return neighbourList;
	}

	private List<GridPosition> CalculatePath(PathNode endNode)
	{
		var path = new List<GridPosition>();
		path.Add(endNode.GridPosition);

		var currentNode = endNode;
		while (currentNode.CameFromNode != null)
		{
			path.Add(currentNode.CameFromNode.GridPosition);
			currentNode = currentNode.CameFromNode;
		}

		path.Reverse();
		return path;
	}

	public void SetIsWalkable(GridPosition gridPosition, bool isWalkable)
	{
		gridSystem.GetGridObject(gridPosition).IsWalkable = isWalkable;
	}
}

