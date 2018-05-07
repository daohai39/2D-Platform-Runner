using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoThroughOnCollision : MonoBehaviour {

	[SerializeField] private Collider2D targetCollider;

    /// <summary>
    /// Sent when an incoming collider makes contact with this object's
    /// collider (2D physics only).
    /// </summary>
    /// <param name="other">The Collision2D data associated with this collision.</param>
    private void OnCollisionEnter2D(Collision2D other)
    {
        Physics2D.IgnoreCollision(GetComponent<Collider2D>(), targetCollider, true);
    }
	
}
