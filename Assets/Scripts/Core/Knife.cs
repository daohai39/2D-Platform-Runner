using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Knife : MonoBehaviour {

	private int id;

	public int Id {get; set;}

	[SerializeField] private float speed;

	private Vector2 direction;

	private Rigidbody2D rgbody; 

	// Use this for initialization
	private void Start () {
		rgbody = GetComponent<Rigidbody2D>();
	}
	
	// Update is called once per frame
	private void Update () {
		rgbody.velocity = speed * direction;
	}

	public void Initialize(Vector2 direction)
	{
		this.direction = direction;
	}

	private void OnBecameInvisible()
	{
		Destroy(gameObject);		
	}

}
