using UnityEngine;

public class UnitRagdoll : MonoBehaviour
{
	[SerializeField] private Transform ragdollRootBone;

	public void Setup(Transform originalRootBone)
	{
		MatchAllChildTransforms(originalRootBone, ragdollRootBone);

		var randomForce = new Vector3(Random.Range(-1f, 1f), 0f, Random.Range(-1f, 1f));
		ApplyExplosionToRagdoll(ragdollRootBone, 300f,transform.position + randomForce, 10f);
	}

	private void ApplyExplosionToRagdoll(Transform root, float explosionForce, Vector3 explosionPosition, float explosionRange)
	{
		foreach (Transform child in root)
		{
			if (child.TryGetComponent<Rigidbody>(out Rigidbody childRigidbody))
			{
				childRigidbody.AddExplosionForce(explosionForce, explosionPosition, explosionRange);
			}

			ApplyExplosionToRagdoll(child, explosionForce, explosionPosition, explosionRange);
		}
	}

	private void MatchAllChildTransforms(Transform original, Transform clone)
	{
		foreach (Transform originalChild in original)
		{
			var cloneChild = clone.Find(originalChild.name);
			if (cloneChild)
			{
				cloneChild.position = originalChild.position;
				cloneChild.rotation = originalChild.rotation;
				MatchAllChildTransforms(originalChild, cloneChild);
			}
		}
	}
}
