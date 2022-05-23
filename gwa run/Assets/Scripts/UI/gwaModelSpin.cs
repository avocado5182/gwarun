using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gwaModelSpin : MonoBehaviour {
    public float spinSpeed = 60f;
    public float rotateSpeed = 20f;
    bool mouseHeld = false;
    // bool mouseIsOver = false;

    // Update is called once per frame
    void Update()
    {
        // if (!mouseHeld) transform.RotateAround(pivotPoint.position, Vector3.up, spinSpeed * Time.deltaTime);
        if (!mouseHeld) transform.Rotate(Vector3.up, spinSpeed * Time.deltaTime, Space.World);
        else {
            float rotX = Input.GetAxis("Mouse X") * rotateSpeed * Mathf.Deg2Rad;
            float rotY = Input.GetAxis("Mouse Y") * rotateSpeed / 10 * Mathf.Deg2Rad;

            transform.Rotate(Vector3.up, -rotX, Space.World);
            transform.Rotate(Vector3.right, rotY, Space.World);
        }
    }

    void OnMouseDown() {
        mouseHeld = true;
    }

    void OnMouseUp() {
        mouseHeld = false;
    }
}
