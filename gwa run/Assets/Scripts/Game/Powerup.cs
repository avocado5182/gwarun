using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Powerup : MonoBehaviour {
	public float duration;
	public UnityEvent onEffect;
	public Sprite effectIcon;
	public Coroutine running;

	public void StopRunning() {
		if (running != null) {
			StopCoroutine(running);
		}
	}
	
	public void DoPowerup() {
		StartCoroutine(DoPowerupCoroutine());
	}

	IEnumerator DoPowerupCoroutine() {
		GameManager.Instance.isPowerupActive = true;
		
		running = StartCoroutine(name);
		yield return running;
		
		// after the powerup
		GameManager.Instance.isPowerupActive = false;
		GameManager.Instance.powerupTimeLeft = 0;
	}

	IEnumerator Star() {
		Debug.Log("STAR GO BRR");
		GameManager.Instance.isStarActive = true; 
		
		// code here
		
		yield return new WaitForSeconds(duration);
		GameManager.Instance.isStarActive = false; 
		Debug.Log("STAR DIGGITY DONE");
		running = null;
	}

	IEnumerator Potion() {
		Debug.Log("POTION GO BRR");
		GameManager.Instance.isTimeSlow = true;
		
		// code here
		
		yield return new WaitForSeconds(duration);
		GameManager.Instance.isTimeSlow = false;
		Debug.Log("POTION DIGGITY DONE");
		running = null;
	}
}