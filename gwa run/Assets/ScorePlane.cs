using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScorePlane : MonoBehaviour {
    public bool scored = false;

    public bool NoSiblings() {
	    return transform.parent.childCount == 1 &&
	           transform.parent.GetChild(0).CompareTag("Score");
    }
}
