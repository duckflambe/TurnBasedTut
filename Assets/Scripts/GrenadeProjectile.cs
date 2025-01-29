using System;
using UnityEngine;
using UnityEngine.UIElements;

public class GrenadeProjectile : MonoBehaviour
{
	[SerializeField] private Transform explosionPrefab;
	[SerializeField] private TrailRenderer trailRenderer;
	[SerializeField] private AnimationCurve arcYAnimationCurve;

	[SerializeField] private float moveSpeed = 15f;
	[SerializeField] private float explosionRadius = 4f;
	[SerializeField] private float arcHeight = 3f;

	public static event EventHandler OnAnyGrenadeExploded;
	private Action onComplete;
	private Vector3 targetPosition;
	private float totalDistance;
	private Vector3 positionXZ;

	private void Update()
	{
		if (targetPosition == null)
		{
			return;
		}

		positionXZ = Vector3.MoveTowards(positionXZ, targetPosition, moveSpeed * Time.deltaTime);
		if (positionXZ == targetPosition)
		{
			Explode();
			onComplete?.Invoke();
			Destroy(gameObject);
		}

		float distanceNormalized = 1f - Vector3.Distance(positionXZ, targetPosition) / totalDistance;
		float y = arcYAnimationCurve.Evaluate(distanceNormalized) * arcHeight;
		transform.position = new Vector3(positionXZ.x, y, positionXZ.z);
	}

	private void Explode()
	{
		Instantiate(explosionPrefab, targetPosition, Quaternion.identity);
		trailRenderer.transform.SetParent(null);

		var colliderArray = Physics.OverlapSphere(targetPosition, explosionRadius);
		foreach (var collider in colliderArray)
		{
			if (collider.TryGetComponent<Unit>(out Unit targetUnit))
			{
				targetUnit.Damage(50);
			}
			else if (collider.TryGetComponent<DestructibleObject>(out DestructibleObject destructibleObject))
			{
				destructibleObject.Damage(50);
			}
		}

		OnAnyGrenadeExploded?.Invoke(this, EventArgs.Empty);
	}

	public void Setup(GridPosition targetGridPosition, Action onComplete)
	{
		targetPosition = LevelGrid.Instance.GetWorldPosition(targetGridPosition);
		positionXZ = transform.position;
		positionXZ.y = 0;
		totalDistance = Vector3.Distance(positionXZ, targetPosition);
		this.onComplete = onComplete;
	}
}
