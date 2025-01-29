using System;
using UnityEngine;

public class UnitRagdollSpawner : MonoBehaviour
{
	[SerializeField] private Transform ragdollPrefab;
	[SerializeField] private Transform originalRootBone;


	private HealthSystem healthSystem;

	private void Awake()
	{
		healthSystem = GetComponent<HealthSystem>();
		healthSystem.OnDeath += HealthSystem_OnDeath;
	}

	private void HealthSystem_OnDeath(object sender, EventArgs e)
	{
		var ragdollTransform = Instantiate(ragdollPrefab, transform.position, transform.rotation);
		var unitRagdoll = ragdollTransform.GetComponent<UnitRagdoll>();
		unitRagdoll.Setup(originalRootBone);
	}
}
