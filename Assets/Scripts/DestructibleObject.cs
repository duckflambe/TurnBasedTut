using UnityEngine;

public class DestructibleObject : MonoBehaviour
{
	[SerializeField] private Transform destroyPrefab;
	[SerializeField] private int maxHealth = 1;
	private int currentHealth;

	private void Awake()
	{
		currentHealth = maxHealth;
	}

	public void Damage(int damage)
	{
		currentHealth -= damage;
		if (currentHealth <= 0)
		{
			gameObject.layer = 0;
			var exploded = Instantiate(destroyPrefab, transform.position, transform.rotation);
			ApplyExplosionToChildren(exploded, transform.position, 10f, 150f);

			Destroy(gameObject);
			
			Pathfinding.Instance.SetupObstacles();
		}
	}

	private void ApplyExplosionToChildren(Transform parent, Vector3 explosionPosition, float explosionRadius, float explosionForce)
	{
		foreach (Transform child in parent)
		{
			Rigidbody rb = child.GetComponent<Rigidbody>();
			if (rb != null)
			{
				rb.AddExplosionForce(explosionForce, explosionPosition, explosionRadius);
			}
			ApplyExplosionToChildren(child, explosionPosition, explosionRadius, explosionForce);
		}
	}

}
