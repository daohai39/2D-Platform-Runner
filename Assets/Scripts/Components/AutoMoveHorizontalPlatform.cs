using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoMoveHorizontalPlatform : MonoBehaviour {
	[SerializeField] private float xMin;
	[SerializeField] private float xMax;
	[SerializeField] private float speed;
	
	private Vector2 direction;
	void Start() {
		direction = Vector2.right;
	}

	void Update() {
		transform.Translate(speed * GetDirection() * Time.deltaTime);
	}

	private Vector2 GetDirection() {
		if (transform.position.x >= xMax) 
			direction = Vector2.left;
		else if (transform.position.x <= xMin)
			direction = Vector2.right;
		return direction;
	}
}
