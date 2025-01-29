using UnityEngine;

public class PathNode
{
	private GridPosition gridPosition;
	private int gCost;
	private int hCost;
	private PathNode cameFromNode;
	private bool isWalkable = true;

	public PathNode(GridPosition gridPosition)
	{
		this.gridPosition = gridPosition;
	}

	public override string ToString()
	{
		return gridPosition.ToString();
	}

	public int GCost
	{
		get => gCost;
		set
		{
			gCost = value;
		}
	}

	public int HCost
	{
		get => hCost;
		set
		{
			hCost = value;
		}
	}

	public int FCost
	{
		get => hCost + gCost;
	}

	public PathNode CameFromNode
	{
		get => cameFromNode;
		set
		{
			cameFromNode = value;
		}
	}

	public GridPosition GridPosition
	{
		get => gridPosition;
	}

	public bool IsWalkable
	{
		get => isWalkable;
		set
		{
			isWalkable = value;
		}
	}
}
