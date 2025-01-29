using TMPro;
using UnityEngine;

public class PathfindingGridDebugObject : GridDebugObject
{
	[SerializeField] private TextMeshPro gCostText;
	[SerializeField] private TextMeshPro hCostText;
	[SerializeField] private TextMeshPro fCostText;
	[SerializeField] private SpriteRenderer isWalkableSprite;

	private PathNode pathNode;

	protected override void Update()
	{
		base.Update();
		if (pathNode != null)
		{
			gCostText.text = pathNode.GCost.ToString();
			hCostText.text = pathNode.HCost.ToString();
			fCostText.text = pathNode.FCost.ToString();

			isWalkableSprite.color = pathNode.IsWalkable ? Color.green : Color.red;
		}
	}

	public override void SetGridObject(object gridObject)
	{
		base.SetGridObject(gridObject);

		pathNode = gridObject as PathNode;
	}
}
