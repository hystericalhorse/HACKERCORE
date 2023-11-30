using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Rendering.Universal;

public class LightController : MonoBehaviour {
    public Light2D ExplosionLight;
    public float ExplosionLightIntensity;

    void Start() {
        DOVirtual.Float(0, ExplosionLightIntensity, .05f, ChangeLight).OnComplete(() => DOVirtual.Float(ExplosionLightIntensity, 0, .1f, ChangeLight));
    }

    void ChangeLight(float x) {
        ExplosionLight.intensity = x;
    }
}