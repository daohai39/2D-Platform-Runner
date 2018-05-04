using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public abstract class Enemy : MonoBehaviour {
	private int id;

	public int Id { get; set; }

	public Animator Animator { get; private set; }

	public Rigidbody2D RigidBody { get; private set; }

	private bool isFacingRight;

	[SerializeField] private int health;

	[SerializeField] private int point;



	// Use this for initialization
	void Start () {
		Animator = GetComponent<Animator>();
		RigidBody = GetComponent<Rigidbody2D>();	
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void ChangeDirection()
	{
		isFacingRight = !isFacingRight;
		transform.localScale = new Vector2(transform.localScale.x * - 1, transform.localScale.y);
	}
}
