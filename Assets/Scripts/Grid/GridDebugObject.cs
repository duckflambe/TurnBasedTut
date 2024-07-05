using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GridDebugObject : MonoBehaviour
{
	private GridObject gridObject;

	private void Update()
	{
		GetComponentInChildren<TextMeshPro>().text = gridObject.ToString();
	}

	public void SetGridObject(GridObject gridObject)
	{
		this.gridObject = gridObject;
	}
}
