using UnityEngine;
using Unity.Cinemachine;
using System;

public class ScreenShake : MonoBehaviour
{
	public static ScreenShake Instance { get; private set; }

	private CinemachineImpulseSource cinemachineInpulseSource;

	private void Awake()
	{
		if (Instance != null)
		{
			Debug.LogWarning("Multiple ScreenShake instances detected, destroying the newest one.");
			Destroy(gameObject);
			return;
		}

		Instance = this;
		cinemachineInpulseSource = GetComponent<CinemachineImpulseSource>();
	}

	private void Start()
	{
		ShootAction.OnAnyShoot += HandleOnAnyShoot;
		GrenadeProjectile.OnAnyGrenadeExploded += HandleOnAnyGrenadeExploded;
	}

	private void HandleOnAnyGrenadeExploded(object sender, EventArgs e)
	{
		Shake(10);
	}

	private void HandleOnAnyShoot(object sender, ShootAction.OnShootEventArgs e)
	{
		Shake();
	}

	public void Shake(float intensity = 1f)
	{
		cinemachineInpulseSource.GenerateImpulse(intensity);
	}
}
