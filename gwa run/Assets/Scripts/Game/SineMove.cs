using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SineMove : MonoBehaviour {
	public float speed;
	public Vector2 range;
	public float init;
	public Axis axis;
	float v;
	float initSpeed;
	
	void Start() {
		initSpeed = speed;
	}

	// Update is called once per frame
	void Update() {
		if (speed == 0) return;
		speed = initSpeed * 
		        ((GameManager.Instance.isTimeSlow) ? 
			        PlayerMovement.Instance.slowSpeed : 
			        1);
		Vector3 pos = transform.position;
		transform.position = new Vector3(
			(axis == Axis.X) ? CurrentPosition() : pos.x,			
			(axis == Axis.Y) ? CurrentPosition() : pos.y,			
			(axis == Axis.Z) ? CurrentPosition() : pos.z	
		);
		
		v += speed * Time.deltaTime;
		// v %= Mathf.Abs(range.x) + Mathf.Abs(range.y);
		v %= 2 * Mathf.PI;
	}

	// something within the range + init
	// like if init is 3 and the range goes from -2 to 4
	// this would return a value between 1 and 7
	// then do transform.position = new Vector3(value, pos.y, pos.z)
	// ^ if your axis is x for example
	float CurrentPosition() {
		float mid = (range.y - range.x) / 2f;
		float pos = Mathf.Sin(v) * mid + mid + range.x + init; 

		return pos;
	}
}