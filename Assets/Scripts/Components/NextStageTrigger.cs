﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class NextStageTrigger : MonoBehaviour {

	void OnTriggerEnter2D(Collider2D other)
	{
		if (other.GetComponent<Player>() != null)
		{
			var currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
			SceneManager.LoadScene(currentSceneIndex+1);
		}
	}	
}
