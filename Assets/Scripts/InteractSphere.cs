using System;
using UnityEngine;

public class InteractSphere : MonoBehaviour, IInteractable
{
	[SerializeField] private Material activatedMaterial;
	[SerializeField] private Material deactivatedMaterial;
	[SerializeField] private MeshRenderer meshRenderer;

	private Action onInteractComplete;
	private bool isActivated = false;
	private bool isActive = false;
	private float timer;

	private GridPosition gridPosition;

	private void Start()
	{
		gridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
		LevelGrid.Instance.SetInteractableAtGridPosition(this, gridPosition);

		UpdateMaterial();
	}

	private void Update()
	{
		if (!isActive)
		{
			return;
		}

		timer -= Time.deltaTime;
		if (timer <= 0)
		{
			isActive = false;
			onInteractComplete();
		}
	}

	public void Interact(Action onInteractComplete)
	{
		this.onInteractComplete = onInteractComplete;
		isActive = true;
		timer = 0.5f;

		isActivated = !isActivated;
		UpdateMaterial();
	}

	private void UpdateMaterial()
	{
		meshRenderer.material = isActivated ? activatedMaterial : deactivatedMaterial;
	}
}
