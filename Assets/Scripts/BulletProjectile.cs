using UnityEngine;

public class BulletProjectile : MonoBehaviour
{
	[SerializeField] private TrailRenderer trailRenderer;
	[SerializeField] private Transform bulletHitFxPrefab;

	private Vector3 targetPosition;

	public void Setup(Vector3 targetPosition)
	{
		this.targetPosition = targetPosition;
	}

	private void Update()
	{
		transform.position = Vector3.MoveTowards(transform.position, targetPosition, Time.deltaTime * 150f);
		if (transform.position == targetPosition)
		{
			var bulletHitFxTransform = Instantiate(bulletHitFxPrefab, targetPosition, Quaternion.identity);

			trailRenderer.transform.SetParent(null);
			Destroy(gameObject);
		}
	}
}
