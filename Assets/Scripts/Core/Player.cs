using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void DeadEventHandler();

public class Player : MonoBehaviour {

	private int _id;
	public int Id { get; private set; }

	public event DeadEventHandler Dead;
	private static Player _instance;

	public static Player Instance {
		get {
			if (_instance == null)
			{
				return GameObject.FindObjectOfType<Player>();
			} 
			return _instance;
		}
	}
		
	[SerializeField] private float speed;

	[SerializeField] private float jumpForce;

	[SerializeField] private float groundRadius;

	//whether or not player can steer while jumping
	[SerializeField] private bool airControl;

	[SerializeField] private float immortalTime;

	[SerializeField] private float deathTime;
	[SerializeField] protected List<string> damageSources;

	[SerializeField] private LayerMask whatIsGround;

	[SerializeField] private List<Transform> groundChecks;
	
	[SerializeField] private GameObject knifePrefab;

	[SerializeField] private Transform knifePos;

	[SerializeField] private Collider2D swordCollider;

	private bool isFacingRight;

	private SpriteRenderer spriteRenderer;

	private Health health;

	public Rigidbody2D Rigidbody {get;private set;}
	
	public Animator Animator {get; private set;}

	private bool isDead;

    public  bool IsDead {
        get { 
			if (health.CurrentHealth <= 0) {
				OnDead();
			}
            return health.CurrentHealth <= 0;
        }
		set {
			if (value == true) {
				health.CurrentHealth = 0;
			} 
			isDead = value;
		}
    }
	public bool Attack { get; set; }	

	public bool Slide { get; set; }

	public bool Running { get; set; }

	public bool OnGround { get; set; }

	public bool Immortal { get; set; }

	public bool Jump { get; set; }

	// Use this for initialization
	private void Start () 
	{
		Id = 0;
		isFacingRight = true;
		health = GetComponent<Health>();
		spriteRenderer = GetComponent<SpriteRenderer>();
		Rigidbody = GetComponent<Rigidbody2D>();
		Animator = GetComponent<Animator>();	
	}
	
	// Update is called once per frame
	private void Update () 
	{
		if(IsDead) return;
		HandleInput();	
	}


	private void FixedUpdate() 
	{	
		if (IsDead) return;
		var horizontal = Input.GetAxis("Horizontal");

		OnGround = IsGrounded();
		if (!Attack) {
			HandleMovement(horizontal);
			
			HandleDirection(horizontal);
		
			HandleLayer();
		}
	}

	private bool IsGrounded() 
	{
		if (Rigidbody.velocity.y <= 0) //whether character is jumping or not
		{
			foreach (Transform groundCheck in groundChecks) {
				Collider2D[] colliders = Physics2D.OverlapCircleAll(groundCheck.position, groundRadius, whatIsGround);
				foreach (Collider2D collider in colliders) {
					if (collider.gameObject != gameObject) return true;
				}
			}
		}
		return false;
	}

	private void HandleMovement(float horizontal)
	{	
		//Whether player is falling
		if (Rigidbody.velocity.y < 0)
		{
			Animator.SetBool("landing", true);
		}
		//Whether player is jumping
		if (OnGround && Jump && !Slide) 
		{
			Rigidbody.AddForce(new Vector2(0, jumpForce), ForceMode2D.Force);
		}
		//Whether player can move
		if (!Attack && !Slide && OnGround || airControl)
		{
			Rigidbody.velocity = new Vector2(horizontal*speed, Rigidbody.velocity.y);
		}

		Animator.SetFloat("speed", Mathf.Abs(horizontal));
	}
	private void HandleInput() 
	{
		if (Input.GetKeyDown(KeyCode.LeftControl)) {
			Animator.SetTrigger("attack");
		}
		if (Input.GetKeyDown(KeyCode.X)) {
			Animator.SetTrigger("rangeAttack");
		}
		if (Input.GetKeyDown(KeyCode.LeftShift)) {
			Animator.SetTrigger("slide");
		}
		if (Input.GetKeyDown(KeyCode.Space)) {
			Animator.SetTrigger("jump");
		}
	}

	private void HandleDirection(float horizontal) 
	{
		if (horizontal > 0 && !isFacingRight || horizontal < 0 && isFacingRight)
		{
			isFacingRight = !isFacingRight;
			transform.localScale = new Vector2 (transform.localScale.x * -1, transform.localScale.y);
		}
	}

	private void HandleLayer()
	{
		if(OnGround) {
			Animator.SetLayerWeight(1,0.0f);
		} else {
			Animator.SetLayerWeight(1,1.0f);
		}
	}


	private void ThrowKnife(int value)
	{
		if (OnGround && value == 0 || !OnGround && value == 1) {
			if (isFacingRight) {
				GameObject tmp = (GameObject)Instantiate(knifePrefab, knifePos.position, Quaternion.Euler(new Vector3(0,0, -90)));
				tmp.GetComponent<RangeWeapon>().Initialize(Vector2.right);
			} else {
				GameObject tmp = (GameObject)Instantiate(knifePrefab, knifePos.position, Quaternion.Euler(new Vector3(0,0, 90)));
				tmp.GetComponent<RangeWeapon>().Initialize(Vector2.left);
			}
		}
 	}

	public void MeleeAttack()
	{
		swordCollider.enabled = !swordCollider.enabled;
	}

    public  IEnumerator TakeDamage(int amount)
    {
		if (!Immortal) {
			health.TakeDamage(amount);
			if (IsDead) {
                Animator.SetLayerWeight(1,0);
				Animator.SetTrigger("die");
			} else {
                Animator.SetLayerWeight(1,0);
				Animator.SetTrigger("damage");
				Immortal = true;
				StartCoroutine(ImmortalState());
				yield return new WaitForSeconds(immortalTime);
				Immortal = false;
			}
		}
    }
	
	public void Respawn()
	{
		health.Reset();
		Animator.ResetTrigger("die");
		Animator.SetTrigger("idle");
		transform.position = Vector2.zero;
		Rigidbody.velocity = Vector2.zero;
	}

	public void OnDead()
	{
		if (Dead != null) {
			Dead();
		}
	}

	private IEnumerator ImmortalState()
	{
		while(Immortal) {
			spriteRenderer.enabled = false;
			yield return new WaitForSeconds(.1f);
			spriteRenderer.enabled = true;
			yield return new WaitForSeconds(.1f);
		}
	}

	private	void OnTriggerEnter2D(Collider2D other)
	{
		if (damageSources.Contains(other.tag)) { //Destroy knife prefab when hit
			StartCoroutine(TakeDamage(other.GetComponent<DamageManagement>().DamageOutput));
			if(other.tag == "EnemyKnife") Destroy(other.gameObject);
		}
		
	}

}
