using System;
using UnityEngine;

public class HealthSystem : MonoBehaviour
{
	public event EventHandler OnDeath;
	public event EventHandler OnDamage;

	[SerializeField] private int maxHealth = 100;
	private int currentHealth;

	private void Awake()
	{
		currentHealth = maxHealth;
	}

	public void TakeDamage(int damage)
	{
		currentHealth -= damage;
		if (currentHealth <= 0)
		{
			currentHealth = 0;
			OnDeath?.Invoke(this, EventArgs.Empty);
		}
		else
		{
			OnDamage?.Invoke(this, EventArgs.Empty);
		}
	}

	public float GetHealthNormalized()
	{
		return (float)currentHealth / maxHealth;
	}
}
