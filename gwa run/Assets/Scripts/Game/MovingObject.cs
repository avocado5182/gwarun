using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingObject : MonoBehaviour {
    PlayerMovement playerMvmt;
    // float distToNewSection;
    // float distToNewObstacle;
    bool spawnedYet;

    void Awake() {
        playerMvmt = FindObjectOfType<PlayerMovement>();
        // distToNewSection = GameManager.Instance.sectionDistance;
        // distToNewObstacle = GameManager.Instance.distanceToNewObstacle;
    }

    void FixedUpdate() {
        if (!playerMvmt || !playerMvmt.movementIsEnabled) return;
        Vector3 pos = transform.position;
        
        Vector3 newPos = new Vector3(
            pos.x,
            pos.y,
            pos.z - playerMvmt.forwardSpeed * Time.fixedDeltaTime * Time.fixedDeltaTime
        );
        
        transform.position = newPos;
    }
}
