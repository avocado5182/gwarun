using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SectionBackgroundSetter : MonoBehaviour {
    public int sectionNumber;

    void OnTriggerEnter(Collider other) {
        // use the section number and bg list in gameMgr
        RenderSettings.skybox = GameManager.Instance.sectionBGs[sectionNumber];
    }
}
