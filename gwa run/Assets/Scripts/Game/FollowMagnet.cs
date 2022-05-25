using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowMagnet : MonoBehaviour {
    public Transform target; // by default, playergfx.transform
    public float minDist;
    public float speed;
    Vector3 initPos;
    public bool hasCollided;
    public bool hasSetPos;

    void Start() {
        if (target == null) target = GameManager.Instance.playerGFX.transform;
        PlayerMovement pMvmt = GameManager.Instance.playerMovement;
        minDist = pMvmt.maxXValue - pMvmt.minXValue;
        hasCollided = false;
        hasSetPos = false;
    }

    public void UpdateCoinChildren() {
        Transform parent = transform.parent;
        FollowMagnet[] coins = parent.GetComponentsInChildren<FollowMagnet>(); // to only get coin objects
        for (int i = 0; i < coins.Length; i++) {
            Transform coin = parent.GetChild(i);
            coin.transform.localPosition = new Vector3(
                0,
                0,
                i * (1f - 1f / coins.Length)
            );
        }
    }
    
    void Update() {
        if (GameManager.Instance.magnet) {
            float dist = Vector3.Distance(transform.position, target.position);
            if (dist <= minDist) { 
                speed = minDist - dist;
                speed *= Time.deltaTime * 25f;
                Vector3 moveTowards = Vector3.MoveTowards(transform.position, new Vector3(
                    target.position.x,
                    target.position.y,
                    target.position.z + 0.25f
                ), speed);
                transform.position = moveTowards;
                
                hasSetPos = true;
            }
        }

        if (!GameManager.Instance.magnet && !hasSetPos) UpdateCoinChildren();
    }
}
