﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public abstract class Enemy : MonoBehaviour {
	private int id;

	public int Id { get; set; }

	public Animator Animator { get; private set; }

	public Rigidbody2D Rigidbody2D { get; private set; }

	public GameObject Target { get; set; }

	public bool Attack { get; set; }

	public abstract bool IsDead { get; }

	protected bool isFacingRight;

	protected IEnemyState currentState;

	[SerializeField] protected int health = 30;

	[SerializeField] private int point;

	[SerializeField] protected float speed;
    [SerializeField] private float throwRange;
    [SerializeField] private float meleeRange;

	[SerializeField] protected List<string> damageSources;

	[SerializeField] private Transform leftEgde;
	[SerializeField] private Transform rightEgde;
    public bool IsInMeleeRange {
        get {
            if(Target == null) return false;
            return Vector2.Distance(Target.transform.position, transform.position) <= meleeRange;
        }
    }
    public bool IsInThrowRange {
        get {
            if(Target == null) return false;
            return Vector2.Distance(Target.transform.position, transform.position) <= throwRange;
        }
    }


	// Use this for initialization
	protected virtual void Start () {
		Animator = GetComponent<Animator>();
		Rigidbody2D = GetComponent<Rigidbody2D>();	
		isFacingRight = true;
		ChangeState(new IdleState());
	}
	

	protected virtual void Update () 
	{
		if(!IsDead){
			currentState.Execute();
			LookAtTarget();
		}
	}


	protected Vector2 GetDirection()
	{
		return isFacingRight ? Vector2.right : Vector2.left;
	}


	protected virtual void LookAtTarget() 
	{
		if (Target != null) {
			var xDir = Target.transform.position.x - transform.position.x;
			if (xDir > 0 && !isFacingRight || xDir < 0 && isFacingRight) {
				ChangeDirection(); 
			}
		}
	}

	protected bool IsOnEdge()
	{
		return (GetDirection().x > 0 && transform.position.x >= rightEgde.position.x 
		|| GetDirection().x < 0 && transform.position.x <= leftEgde.position.x);
	}

	public void ChangeState(IEnemyState newState) 
	{
		if (newState == null)
			return;
		if (currentState != null)
			currentState.Exit();
		currentState = newState;
		currentState.Enter(this);
	}

	public void ChangeDirection()
	{
		isFacingRight = !isFacingRight;
		transform.localScale = new Vector2(transform.localScale.x * - 1, transform.localScale.y);
	}

	public virtual void DestroySelf() 
	{
		gameObject.SetActive(false);
	}

	private	void OnTriggerEnter2D(Collider2D other)
	{
		if (IsDead) return;
		if (damageSources.Contains(other.tag)) {
			if(other.tag == "Knife") Destroy(other.gameObject); // Destroy knife prefab or else it will collide with enemy sight
			StartCoroutine(TakeDamage());
		}
		
	}

	// public abstract void Idle();
	public abstract void Move();	
	public abstract void PerformAttack();

	public abstract void Die();
	
	public abstract IEnumerator TakeDamage();

}
