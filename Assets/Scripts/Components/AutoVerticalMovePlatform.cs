using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoVerticalMovePlatform : MonoBehaviour {
	[SerializeField] private float yMin;
	[SerializeField] private float yMax;
	[SerializeField] private float speed;
	
	private Vector2 direction;
	void Start() {
		direction = Vector2.up;
	}

	void Update() {
		transform.Translate(speed * GetDirection() * Time.deltaTime);
	}

	private Vector2 GetDirection() {
		if (transform.position.y >= yMax) 
			direction = Vector2.down;
		else if (transform.position.y <= yMin)
			direction = Vector2.up;
		return direction;
	}
}
