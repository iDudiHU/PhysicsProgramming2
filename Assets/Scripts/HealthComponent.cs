using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Events;
using UnityStandardAssets.Characters.ThirdPerson;

/// <summary>
/// This class handles the health state of a game object.
/// </summary>\
public class HealthComponent : MonoBehaviour
{
	public static Action OnHealthLost;
	[Header("=== Team Settings ===")]
	[Tooltip("The team associated with this damage")]
	[SerializeField] int teamId = 0;

	[Header("=== Health Settings ===")]
	[Tooltip("The default health value")]
	[SerializeField] float defaultHealth = 100.0f;
	[Tooltip("The maximum health value")]
	[SerializeField] float maximumHealth = 100.0f;
	[Tooltip("The current in game health value")]
	[SerializeField] float currentHealth = 100.0f;
	[field: SerializeField] public float CurrentHealth { get; private set; } = 100.0f;
	[Tooltip("Invulnerability duration, in seconds, after taking damage")]
	[SerializeField] float invincibilityTime = 0.0f;
	[Tooltip("Whether or not this health is always invincible")]
	[SerializeField] bool isAlwaysInvincible = false;

	[Header("=== Lives settings ===")]
	[Tooltip("Whether or not to use lives")]
	[SerializeField] bool useLives = false;
	[Tooltip("Current number of lives this health has")]
	[SerializeField] int currentLives = 3;
	[Tooltip("The maximum number of lives this health has")]
	[SerializeField] int maximumLives = 5;
	[Tooltip("The amount of time to wait before respawning")]
	[SerializeField] float respawnWaitTime = 1f;
	private float respawnTime;
	// The specific game time when the health can be damged again
	private float timeToBecomeDamagableAgain = 0;
	// Whether or not the health is invincible
	private bool isInvincableFromDamage = false;

	[Header("=== Respawn Settings ===")]
	[SerializeField] Vector3 respawnPosition;
	[SerializeField] Quaternion respawnRotation;

	[Header("Effects & Polish")]
	[Tooltip("The effect to create when this health dies")]
	public GameObject deathEffect;
	[Tooltip("The effect to create when this health is damaged (but does not die)")]
	public GameObject hitEffect;
	[Tooltip("A list of events that occur when the health becomes 0 or lower")]
	public UnityEvent eventsOnDeath;
	[Tooltip("A list of events that occur when the health becomes 0 or lower")]
	public UnityEvent eventsOnHit;
	[Tooltip("A list of events that occur on respawn")]
	public UnityEvent eventsOnRespawn;

	[Header("DEBUG")]
	[SerializeField] bool debugMode = false;
	void Start()
	{
		SetRespawnPoint(transform.position, transform.rotation);
	}

	void Update()
	{
		//InvincibilityCheck();
		//RespawnCheck();
	}
	private void RespawnCheck()
	{
		if (respawnWaitTime != 0 && currentHealth <= 0 && currentLives > 0)
		{
			if (Time.time >= respawnTime)
			{
				Respawn();
			}
		}
	}
	private void InvincibilityCheck()
	{
		if (timeToBecomeDamagableAgain <= Time.time)
		{
			isInvincableFromDamage = false;
		}
	}
	public void SetRespawnPoint(Vector3 newRespawnPosition, Quaternion newRespawnRotation)
	{
		respawnPosition = newRespawnPosition;
		respawnRotation = newRespawnRotation;
	}
	void Respawn()
	{
		transform.position = respawnPosition;
		transform.rotation = respawnRotation;
		currentHealth = maximumHealth;
	}
	public void TakeDamage(float ammount)
	{
		if (isInvincableFromDamage || currentHealth <= 0 || isAlwaysInvincible)
		{
			return;
		}
		else
		{
			if (hitEffect != null)
			{
				Instantiate(hitEffect, transform.position, transform.rotation, null);
			}
			OnHealthLost?.Invoke();
			eventsOnHit?.Invoke();
			timeToBecomeDamagableAgain = Time.time + invincibilityTime;
			isInvincableFromDamage = true;
			currentHealth = Mathf.Clamp(currentHealth - ammount, 0, maximumHealth);
			CheckDeath();
		}
	}
	public void ReceiveHealing(float ammount)
	{
		currentHealth += ammount;
		if (currentHealth > maximumHealth)
		{
			currentHealth = maximumHealth;
		}
		CheckDeath();
	}
	public void AddLives(int ammount)
	{
		if (useLives)
		{
			currentLives += ammount;
			if (currentLives > maximumLives)
			{
				currentLives = maximumLives;
			}
		}
	}

	public void AddMaxLives()
	{
		maximumLives++;
		currentLives++;
	}

	public void AddMaxHealth(float ammount)
	{
		maximumHealth += ammount;
		currentHealth = maximumHealth;
	}
	public void AddMaxHealthBasedOnLevel(int level)
	{
		maximumHealth += 20 * Mathf.Pow(1.3f, level);
		currentHealth = maximumHealth;
	}
	bool CheckDeath()
	{
		if (currentHealth <= 0)
		{
			Die();
			return true;
		}
		return false;
	}
	void Die()
	{
		if (deathEffect != null)
		{
			if (deathEffect != null)
			{
				Instantiate(deathEffect, transform.position, transform.rotation, null);
			}
		}

		// Do on death events
		if (eventsOnDeath != null)
		{
			eventsOnDeath.Invoke();
		}

		if (useLives)
		{
			currentLives -= 1;
			if (currentLives > 0)
			{
				if (respawnWaitTime == 0)
				{
					Respawn();
				}
				else
				{
					respawnTime = Time.time + respawnWaitTime;
				}
			}
			else
			{
				if (respawnWaitTime != 0)
				{
					respawnTime = Time.time + respawnWaitTime;
				}
				GameOver();
			}

		}
		else
		{
			GameOver();
		}
	}

	public void GameOver()
	{
		//GameManager.GameOver();
	}

	void Log(object message)
	{
		if(debugMode)
		Debug.Log(message);
	}
	//public void Load(GameData.HealthData healthData)
	//{
	//	currentHealth = healthData._currentHealth;
	//	maximumHealth = healthData._maxHealth;
	//	currentLives = healthData._currentLives;
	//	//Set RespawnPoint
	//	SetRespawnPoint(healthData.respawnPoint.position, healthData.respawnPoint.rotation);
	//	GameManager.UpdateUIElements();
	//}

	//public void Save(ref GameData data)
	//{
	//	data.player.healthData._currentHealth = currentHealth;
	//	data.player.healthData._maxHealth = maximumHealth;
	//	data.player.healthData._currentLives = currentLives;
	//	data.player.healthData.respawnPoint.position = respawnPosition;
	//	data.player.healthData.respawnPoint.rotation = respawnRotation;
	//}
}

