using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleGroup : MonoBehaviour {
    public int weight = 5;

    public void OnTransformChildrenChanged() {
        if (transform.childCount == 1) {
            ScorePlane sp = transform.GetChild(0).GetComponent<ScorePlane>();
            if (sp.CompareTag("Score") && sp.scored) Destroy(gameObject);
        }
    }
}
