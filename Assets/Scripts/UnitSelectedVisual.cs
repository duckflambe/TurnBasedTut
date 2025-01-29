using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class UnitSelectedVisual : MonoBehaviour
{
    [SerializeField]
    private Unit selectedUnit;

    private MeshRenderer meshRenderer;

    private void Awake()
    {
        meshRenderer = GetComponent<MeshRenderer>();
    }

    private void Start()
    {
        UnitActionSystem.Instance.OnUnitSelected += UnitActionSystem_OnUnitSelected;
        ShowIfSelected();
    }

	private void OnDestroy()
	{
		UnitActionSystem.Instance.OnUnitSelected -= UnitActionSystem_OnUnitSelected;
	}

	private void UnitActionSystem_OnUnitSelected(object sender, EventArgs e)
    {
        ShowIfSelected();
    }

    private void ShowIfSelected()
    {
        if (UnitActionSystem.Instance.SelectedUnit == selectedUnit)
        {
            meshRenderer.enabled = true;
        }
        else
        {
            meshRenderer.enabled = false;
        }
    }
}
