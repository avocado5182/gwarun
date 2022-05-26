using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BallMover : MonoBehaviour {
    public List<GameObject> balls;
    public float speed;
    float t;
    float initSpeed;

    void Start() {
        initSpeed = speed;
    }

    // Update is called once per frame
    void Update() {
        if (speed == 0) return;
        speed = initSpeed /
                ((GameManager.Instance.isTimeSlow) ? 
                    PlayerMovement.Instance.slowSpeed : 
                    1);
        if (balls.All(ball => ball != null)) {
            for (int i = 0; i < balls.Count; i++) {
                GameObject ball = balls[i];
                Vector3 pos = ball.transform.position;
                ball.transform.position = new Vector3(
                    pos.x,
                    2 * Mathf.Cos((20 * t) / (Mathf.PI)) / (Mathf.Pow(2, i)) + 2.25f,
                    pos.z
                );
            }

            t += Time.deltaTime / speed;
            t %= 1;
        }
    }
}
