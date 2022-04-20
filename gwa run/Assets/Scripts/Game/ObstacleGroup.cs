using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleGroup : MonoBehaviour {
    public int weight = 5;

    public void OnTransformChildrenChanged() {
        if (transform.childCount == 1) {
            if (transform.GetChild(0).CompareTag("Score")) Destroy(gameObject);
        }
    }
}
