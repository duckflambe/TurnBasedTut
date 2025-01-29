using System;
using UnityEngine;

public class Door : MonoBehaviour, IInteractable
{
	[SerializeField] bool isOpen = false;

	private Action onInteractComplete;
	private bool isActive = false;
	private float timer;
	private GridPosition gridPosition;
	private Animator animator;

	private void Awake()
	{
		animator = GetComponent<Animator>();
	}

	private void Start()
	{
		gridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
		LevelGrid.Instance.SetInteractableAtGridPosition(this, gridPosition);

		UpdateState();
	}

	private void Update()
	{
		if(!isActive)
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
		timer = 0.5f;
		isActive = true;

		isOpen = !isOpen;
		UpdateState();
	}

	private void UpdateState()
	{
		animator.SetBool("IsOpen", isOpen);
		Pathfinding.Instance.SetIsWalkable(gridPosition, isOpen);
	}
}
