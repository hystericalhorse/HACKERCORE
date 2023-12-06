using DG.Tweening.Plugins;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;



public class ShieldDisplay : MonoBehaviour
{
	[SerializeField] TextMeshProUGUI text;
	int[] intChars = { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };
	string[] displays = { "255", "255", "255", "255" };

	public void Start()
	{
		text.text = $"{displays[0]}:{displays[1]}:{displays[2]}:{displays[3]}";
	}

	public void DamageShield()
	{
		StartCoroutine(ScrambleDisplay(4 - GameManager.GM.Player.Shield, "0"));
	}

	public void FixShield()
	{
		StartCoroutine(ScrambleDisplay(4 - GameManager.GM.Player.Shield, "255", 0.5f));
	}

	public void UpdateShield()
	{
		text.text = $"{displays[0]}:{displays[1]}:{displays[2]}:{displays[3]}";
	}

	private IEnumerator ScrambleDisplay(int i, string display, float time = 1)
	{
		float timer = time;
		while (timer > 0)
		{
			var x = intChars[UnityEngine.Random.Range(0, 9)].ToString();
			var y = intChars[UnityEngine.Random.Range(0, 9)].ToString();
			var z = intChars[UnityEngine.Random.Range(0, 9)].ToString();
			displays[i] = x + y + z;
			UpdateShield();
			timer -= Time.deltaTime;
			yield return null;
		}

		displays[i] = display;
		UpdateShield();
	}
}