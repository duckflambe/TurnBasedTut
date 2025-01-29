using System;
using UnityEngine;

public class UnitAnimator : MonoBehaviour
{
	[SerializeField] private Animator animator;
	[SerializeField] private Transform bulletProjectilePrefab;
	[SerializeField] private Transform shootPoint;
	[SerializeField] private Transform rifleTransform;
	[SerializeField] private Transform swordTransform;

	private void Awake()
	{
		if (TryGetComponent<MoveAction>(out var moveAction))
		{
			moveAction.OnStartMoving += MoveAction_OnStartMoving;
			moveAction.OnStopMoving += MoveAction_OnStopMoving;
		}

		if (TryGetComponent<ShootAction>(out var shootAction))
		{
			shootAction.OnShoot += ShootAction_OnShoot;
		}

		if (TryGetComponent<SwordAction>(out var swordAction))
		{
			swordAction.OnActionStarted += SwordAction_OnActionStarted;
			swordAction.OnActionCompleted += SwordAction_OnActionCompleted;
		}
	}

	private void Start()
	{
		EquipRifle();
	}

	private void SwordAction_OnActionStarted(object sender, EventArgs e)
	{
		EquipSword();
		animator.SetTrigger("SwordSwing");
	}

	private void SwordAction_OnActionCompleted(object sender, EventArgs e)
	{
		EquipRifle();
	}

	private void ShootAction_OnShoot(object sender, ShootAction.OnShootEventArgs e)
	{
		animator.SetTrigger("Shoot");
		var bulletTransform = Instantiate(bulletProjectilePrefab, shootPoint.position, shootPoint.rotation);
		var bulletScript = bulletTransform.GetComponent<BulletProjectile>();

		var targetPosition = e.targetUnit.GetWorldPosition();
		targetPosition.y += e.targetUnit.GetHeight() * 0.75f;
		bulletScript.Setup(targetPosition);
	}

	private void MoveAction_OnStartMoving(object sender, EventArgs e)
	{
		animator.SetBool("IsMoving", true);
	}

	private void MoveAction_OnStopMoving(object sender, EventArgs e)
	{
		animator.SetBool("IsMoving", false);
	}

	private void EquipSword()
	{
		rifleTransform.gameObject.SetActive(false);
		swordTransform.gameObject.SetActive(true);
	}

	private void EquipRifle()
	{
		rifleTransform.gameObject.SetActive(true);
		swordTransform.gameObject.SetActive(false);
	}
}
