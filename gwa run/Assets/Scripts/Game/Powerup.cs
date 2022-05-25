﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Powerup : MonoBehaviour {
	public float duration;
	public UnityEvent onEffect;
	public Coroutine running;

	public void StopRunning() {
		if (running != null) {
			StopCoroutine(running);
		}
	}
	
	public void DoPowerup() {
		running = StartCoroutine(name);
	}

	IEnumerator Star() {
		Debug.Log("STAR GO BRR");
		yield return new WaitForSeconds(duration);
		Debug.Log("STAR DIGGITY DONE");
		running = null;
	}

	public IEnumerator Magnet() {
		Debug.Log("MAGNET GO BRR");
		yield return new WaitForSeconds(duration);
		Debug.Log("MAGNET DIGGITY DONE");
		running = null;
	}
}