using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingObject : MonoBehaviour {
    PlayerMovement playerMvmt;

    public bool moving = false;
    // float distToNewSection;
    // float distToNewObstacle;
    bool spawnedYet;

    void Awake() {
        playerMvmt = FindObjectOfType<PlayerMovement>();
        // distToNewSection = GameManager.Instance.sectionDistance;
        // distToNewObstacle = GameManager.Instance.distanceToNewObstacle;
    }
    
    // public void OnTransformChildrenChanged() {
    //     if (transform.childCount == 1) {
    //         if (transform.GetChild(0).CompareTag("SpawnPlane")) Destroy(gameObject);
    //     }
    // }

    void FixedUpdate() {
        if (!playerMvmt || !playerMvmt.movementIsEnabled || !moving) return;
        Vector3 pos = transform.position;
        
        Vector3 newPos = new Vector3(
            pos.x,
            pos.y,
            pos.z - playerMvmt.forwardSpeed * Time.fixedDeltaTime * Time.fixedDeltaTime
        );
        
        transform.position = newPos;
    }
}
