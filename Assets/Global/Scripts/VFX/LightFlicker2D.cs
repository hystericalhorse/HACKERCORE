using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;
using UnityEngine.Rendering.Universal;

public class LightFlicker2D : MonoBehaviour
{
    [SerializeField] new Light2D light = null;
    [SerializeField] float transitionTime = 3;
    int i = 0;
    [SerializeField] float minIntensity = 1;
    [SerializeField] float maxIntensity = 10;
	[Range(1, 50)]
	public int smoothing = 5;

	// Continuous average calculation via FIFO queue
	// Saves us iterating every time we update, we just change by the delta
	Queue<float> smoothQueue;
	float lastSum = 0;

	// Start is called before the first frame update
	void Awake()
    {
        if (light == null) light = gameObject.GetComponent<Light2D>();

        i = 0;

        if (maxIntensity < minIntensity) maxIntensity = minIntensity + 1;
        light.intensity = minIntensity;

		smoothQueue = new Queue<float>(smoothing);
	}

	// Update is called once per frame
	void Update()
    {
        if (light is null) return;

        Flicker();
    }

    void Flicker()
    {
		// pop off an item if too big
		while (smoothQueue.Count >= smoothing)
		{
			lastSum -= smoothQueue.Dequeue();
		}

		// Generate random new item, calculate new average
		float newVal = Random.Range(minIntensity, maxIntensity);
		smoothQueue.Enqueue(newVal);
		lastSum += newVal;

		// Calculate new smoothed average
		light.intensity = lastSum / smoothQueue.Count;
	}
}
