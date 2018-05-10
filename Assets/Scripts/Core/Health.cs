using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour {

	[SerializeField] private int maxHealth = 100;
	[SerializeField] private RectTransform healthBar;
	private int currentHealth;

	public int CurrentHealth { 
		get {
			return currentHealth;
		}
		set {
			if (maxHealth < value)
				currentHealth = maxHealth;
			currentHealth = value;
		} 
	}

	// Use this for initialization
	void Start () {
		currentHealth = maxHealth;
	}

	public void TakeDamage(int amount)
	{
		currentHealth -= amount;
		if (currentHealth <= 0) {
			currentHealth = 0;
		}
		if (healthBar != null)  {
			healthBar.sizeDelta = new Vector2(
				currentHealth,
				healthBar.sizeDelta.y
			);
		}
	}

	public void Reset() 
	{
		currentHealth = maxHealth;
	}
}
