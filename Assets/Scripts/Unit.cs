using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
	private GridPosition gridPosition;
	private MoveAction moveAction;
	private SpinAction spinAction;
	private BaseAction[] baseActions;

	private void Awake()
	{
		moveAction = GetComponent<MoveAction>();
		spinAction = GetComponent<SpinAction>();

		baseActions = GetComponents<BaseAction>();
	}

	private void Start()
	{
		gridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
		LevelGrid.Instance.AddUnitToGridPosition(this, gridPosition);
	}

	void Update()
	{
		GridPosition nextGridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
		if(nextGridPosition != gridPosition)
		{
			LevelGrid.Instance.MoveUnit(this, gridPosition, nextGridPosition);
			gridPosition = nextGridPosition;
		}
	}

	public MoveAction GetMoveAction()
	{
		return moveAction;
	}

	public SpinAction GetSpinAction()
	{
		return spinAction;
	}

	public GridPosition GetGridPosition()
	{
		return gridPosition;
	}

	public BaseAction[] GetBaseActions()
	{
		return baseActions;
	}
}
