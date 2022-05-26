using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkyboxRotator : MonoBehaviour
{
    float t = 0;
    public float speed;
    Material skyboxMat;

    // Start is called before the first frame update
    void Start() {
        skyboxMat = RenderSettings.skybox;
    }

    // Update is called once per frame
    void Update() {
        if (speed == 0) return;
        skyboxMat.SetFloat("_Rotation", t);
        t += Time.deltaTime * speed;
        t %= 360;
    }
}
