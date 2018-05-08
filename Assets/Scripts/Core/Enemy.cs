using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public abstract class Enemy : MonoBehaviour {
	private int id;

	public int Id { get; set; }

	public Animator Animator { get; private set; }

	public Rigidbody2D Rigidbody2D { get; private set; }

	public GameObject Target { get; set; }

	public bool Attack { get; set; }

	protected bool isFacingRight;

	private IEnemyState currentState;

	[SerializeField] private int health;

	[SerializeField] private int point;

	[SerializeField] protected float speed;
    [SerializeField] private float throwRange;
    [SerializeField] private float meleeRange;
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
		currentState.Execute();
		LookAtTarget();
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
	// public abstract void Idle();
	public abstract void Move();	
	// public abstract void PerformAttack();

}
