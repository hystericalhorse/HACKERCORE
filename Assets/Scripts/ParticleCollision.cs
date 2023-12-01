using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Cinemachine;

public class ParticleCollision : MonoBehaviour {
    private ParticleSystem Particle;
    public List<ParticleCollisionEvent> CollisionEvents;
    public CinemachineVirtualCamera VirtualCamera;
    public GameObject explosionPrefab;

    void Start() {
        Particle = GetComponent<ParticleSystem>();
        CollisionEvents = new List<ParticleCollisionEvent>();
    }

    void OnParticleCollision(GameObject other) {
        int numCollisionEvents = Particle.GetCollisionEvents(other, CollisionEvents);

        GameObject explosion = Instantiate(explosionPrefab, CollisionEvents[0].intersection, Quaternion.identity);

        ParticleSystem p = explosion.GetComponent<ParticleSystem>();
        var pmain = p.main;

        VirtualCamera.GetComponent<CinemachineImpulseSource>().GenerateImpulse();

        if (other.GetComponent<Rigidbody2D>() != null)
            other.GetComponent<Rigidbody2D>().AddForceAtPosition(CollisionEvents[0].intersection * 10 - transform.position, CollisionEvents[0].intersection + Vector3.up);

    }
}