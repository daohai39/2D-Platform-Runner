using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutOfGameTrigger : MonoBehaviour {
	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.GetComponent<Player>() != null)
		{
			Player.Instance.CurrentHealth = 0;
			Player.Instance.Animator.SetTrigger("die");
		}
	}
}
