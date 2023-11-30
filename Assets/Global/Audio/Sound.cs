using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Sound
{
	public string name;

	[HideInInspector] public AudioSource source;
	public AudioClip clip;

	[Range(0, 1)] public float volume = 0.5f;
	[Range(0, 1)] public float pitch = 0.5f;

	public bool loop = false;
}
