using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IgnoreOnTrigger : MonoBehaviour {
	[SerializeField] private Collider2D ignoreCollider;
	// Use this for initialization
	void Start () {
		Physics2D.IgnoreCollision(GetComponent<Collider2D>(), ignoreCollider, true);
	}
	
}
