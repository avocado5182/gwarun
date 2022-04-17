using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorChanger : MonoBehaviour {
    public Gradient gradient;
    float c; // = 0
    public float speed = 0.2f;
    MeshRenderer mr;

    void Start() {
        mr = GetComponent<MeshRenderer>();
    }

    // Update is called once per frame
    void Update() {
        Color color = gradient.Evaluate(c);
        mr.material.color = color;
        mr.material.EnableKeyword("_EMISSION");
        mr.material.SetColor("_EmissionColor", color);

        c += (Time.deltaTime / speed);
        c %= 1;
    }
}
