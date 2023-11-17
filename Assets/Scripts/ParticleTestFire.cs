using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleTestFire : MonoBehaviour {
    ParticleSystem Gun;

    void Start() {
        Gun = GetComponentInChildren<ParticleSystem>();
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.Space)) {
            Gun.Play();
        } else if (Input.GetKeyUp(KeyCode.Space)) {
            Gun.Stop();
        }
    }
}