using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Axis {
    X = 0,
    Y = 1,
    Z = 2
}

public class ModelSpin : MonoBehaviour {
    public float speed;
    public Axis axis;
    
    // Update is called once per frame
    void Update() {
        if (speed == 0) return;
        switch (axis) {
            case Axis.X:
                transform.Rotate(Vector3.left, speed * Time.deltaTime);
                break;
            case Axis.Y:
                transform.Rotate(Vector3.up, speed * Time.deltaTime);
                break;
            case Axis.Z:
                transform.Rotate(Vector3.forward, speed * Time.deltaTime);
                break;
        }
    }
}
