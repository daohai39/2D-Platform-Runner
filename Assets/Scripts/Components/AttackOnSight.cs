using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackOnSight : MonoBehaviour {
	[SerializeField] private Enemy enemy;
	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.gameObject == Player.Instance.gameObject) {
			enemy.Target = other.gameObject;
		}
	}

	void OnTriggerExit2D(Collider2D other)
	{
		if (other.gameObject == Player.Instance.gameObject) {
			enemy.Target = null;
		}
	}
}
