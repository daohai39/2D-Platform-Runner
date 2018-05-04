using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

	[SerializeField] private GameObject player;
	[SerializeField] private Boundary boundary;
	private Vector3 distance;
	// Use this for initialization
	void Start () {
		distance = player.transform.position - transform.position;
	}
	
	// Update is called once per frame
	void LateUpdate () {
		transform.position = player.transform.position - distance;
		transform.position = new Vector3(
			Mathf.Clamp(transform.position.x, boundary.xMin, boundary.xMax),
			Mathf.Clamp(transform.position.y, boundary.yMin, boundary.yMax),
			transform.position.z
		);
	}
}
