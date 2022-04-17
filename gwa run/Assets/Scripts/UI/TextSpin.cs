using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextSpin : MonoBehaviour {
    public float spinSpeed = 5f;

    [SerializeField] float angle = 0f;
    [SerializeField] float rotValue = 0f;
    [SerializeField] float scaleValue = 0f;
    // Update is called once per frame
    void Update() {
        rotValue = Mathf.Cos(angle) * 15f;
        angle += spinSpeed * Time.deltaTime;
        angle %= 2f * Mathf.PI;

        Quaternion rotation = transform.rotation;
        rotation = Quaternion.Euler(
            rotation.eulerAngles.x,
            rotation.eulerAngles.y,
            rotValue
        );
        transform.rotation = rotation;

        scaleValue = ((Mathf.Cos(angle * 4f) / 8f) + 0.875f) * 16.60399f;
        transform.localScale = new Vector3(
            scaleValue,
            scaleValue,
            1f
        );
    }
}
