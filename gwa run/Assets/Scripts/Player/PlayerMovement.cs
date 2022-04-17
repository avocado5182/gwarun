using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {
    [HideInInspector] public bool movementIsEnabled = true;
    public Rigidbody rb;
    public Transform playerGFX;
    public Transform playerCamera;
    public Vector3 cameraOffset;
    public float forwardSpeed = 5f;
    public float initForwardSpeed = 800f;
    public float accelerationSpeed = 2f;
    public float mouseSensitivity = 5f;
    // public float xSpeed = 2f;
    [Header("")]
    public float minXValue = -2;
    public float maxXValue = 2;

    float mx;
    
    // Start is called before the first frame update
    void Start() {
        if (forwardSpeed != initForwardSpeed) forwardSpeed = initForwardSpeed;
        
        transform.position = new Vector3(0f, 0.5f, 0.5f);
    }

    // Update is called once per frame
    void Update() {
        if (movementIsEnabled) {
            if (forwardSpeed <= 5000) {
                forwardSpeed += (accelerationSpeed * Time.deltaTime) / 1.67f;
            }
            
            TouchPhase touchPhase = (Input.touchCount > 0) ? Input.GetTouch(0).phase : TouchPhase.Stationary;

            bool touchDown = touchPhase == TouchPhase.Moved || Input.GetMouseButton(0);
            bool touchUp = touchPhase == TouchPhase.Ended || Input.GetMouseButtonUp(0);
            mx = (touchDown && !touchUp) ? Input.GetAxisRaw("Mouse X") : 0f;
        }
    }

    void FixedUpdate() {
        if (movementIsEnabled) {
            Vector3 position = playerGFX.position;

            Vector3 movePosition = new Vector3(
                Mathf.Clamp(position.x + (mx * mouseSensitivity), minXValue, maxXValue),
                position.y,
                position.z
            );
            
            // rb.MovePosition(movePosition);
            transform.position = Vector3.Lerp(position, movePosition, Mathf.Abs(mx) * 100f * Time.fixedDeltaTime);
            
            // playerCamera.position = rb.position + cameraOffset;
            playerCamera.position = transform.position + cameraOffset;
        }
    }
}
