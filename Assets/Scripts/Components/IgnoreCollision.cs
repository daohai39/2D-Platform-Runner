using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IgnoreCollision : MonoBehaviour {

	[SerializeField] private BoxCollider2D triggerPlatform;
	[SerializeField] private BoxCollider2D collisionPlatform;
	// Use this for initialization
	private BoxCollider2D playerCollider;
	void Start () 
	{
		playerCollider = Player.Instance.GetComponent<BoxCollider2D>();
		Physics2D.IgnoreCollision(collisionPlatform, triggerPlatform, true);
	}
	
	void OnTriggerEnter2D(Collider2D other)
	{
		if (other.GetComponent<Player>() != null) {
			Physics2D.IgnoreCollision(playerCollider, collisionPlatform, true);
		}
	}

	void OnTriggerExit2D(Collider2D other)
	{
		if(other.GetComponent<Player>() != null) {
			Physics2D.IgnoreCollision(playerCollider, collisionPlatform, false);
		}
	}
}
