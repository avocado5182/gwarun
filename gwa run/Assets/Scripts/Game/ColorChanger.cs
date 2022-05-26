using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorChanger : MonoBehaviour {
    public Gradient gradient;
    float c; // = 0
    public float speed = 0.2f;
    float initSpeed;
    MeshRenderer mr;

    void Start() {
        mr = GetComponent<MeshRenderer>();
        initSpeed = speed;
    }

    // Update is called once per frame
    void Update() {
        if (speed == 0) return;
        speed = initSpeed / 
                ((GameManager.Instance.isTimeSlow) ? 
                    PlayerMovement.Instance.slowSpeed : 
                    1);
        Color color = gradient.Evaluate(c);
        mr.material.color = color;
        mr.material.EnableKeyword("_EMISSION");
        mr.material.SetColor("_EmissionColor", color);

        c += (Time.deltaTime / speed);
        c %= 1;
    }
}
