using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementConstraint : MonoBehaviour {
	[SerializeField] private Boundary boundary;
	private Player player;

	/// <summary>
	/// Start is called on the frame when a script is enabled just before
	/// any of the Update methods is called the first time.
	/// </summary>
	void Start()
	{
		player = GetComponent<Player>();	
	}
	/// <summary>
	/// Update is called every frame, if the MonoBehaviour is enabled.
	/// </summary>
	void Update()
	{
		player.transform.position = new Vector2(
			Mathf.Clamp(player.transform.position.x, boundary.xMin, boundary.xMax),
			Mathf.Clamp(player.transform.position.y, boundary.yMin, boundary.yMax)
		);
	}

}
