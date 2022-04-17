using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour {

    void OnBecameInvisible() {
        // GetComponent<MeshRenderer>().enabled = false;
        // // GetComponent<MeshFilter>().enabled = false;
        // GetComponent<Collider>().enabled = false;
        
        Destroy(gameObject);
    }
}
