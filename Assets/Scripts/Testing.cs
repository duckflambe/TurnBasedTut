using UnityEngine;

public class Testing : MonoBehaviour
{
	void Start()
	{
	}

	void Update()
	{
		if (Input.GetKeyDown(KeyCode.T))
		{
			var mouseGridPosition = LevelGrid.Instance.GetGridPosition(MouseWorld.GetPosition());
			var start = new GridPosition(0, 0);

			var (path, _) = Pathfinding.Instance.FindPath(start, mouseGridPosition);

			for (int i = 0; i < path.Count - 1; i++)
			{
				Debug.DrawLine(
					LevelGrid.Instance.GetWorldPosition(path[i]),
					LevelGrid.Instance.GetWorldPosition(path[i + 1]),
					Color.red, 10f);
			}
		}
	}
}
