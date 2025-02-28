using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridObject
{
	private GridSystem<GridObject> gridSystem;
	private GridPosition position;

	private List<Unit> unitList;
	private IInteractable interactable;

	public IInteractable Interactable { get => interactable; set => interactable = value; }

	public GridObject(GridSystem<GridObject> gridSystem, GridPosition position)
	{
		this.gridSystem = gridSystem;
		this.position = position;
		unitList = new List<Unit>();
	}

	public override string ToString()
	{
		string unitString = "";
        foreach (var item in unitList)
        {
            unitString += item.ToString() + "\n";
        }

        return position.ToString() + "\n" + unitString;
	}

	public GridPosition GetGridPosition()
	{
		return position;
	}

	public void AddUnit(Unit unit)
	{
		unitList.Add(unit);
	}

	public void RemoveUnit(Unit unit)
	{
		unitList.Remove(unit);
	}

	public List<Unit> GetUnitList()
	{
		return unitList;
	}

	public bool HasUnit(Unit unit)
	{
		return unitList.Contains(unit);
	}
}
