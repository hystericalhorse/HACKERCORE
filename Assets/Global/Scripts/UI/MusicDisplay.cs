using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;

public class MusicDisplay : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI text;
    [SerializeField] float scrollSpeed = 1.0f;
	public string display = string.Empty;
	public float timer;

	private void Start()
	{
		timer = scrollSpeed / display.Length;
	}

	private void Update()
	{
		if (timer > 0) timer -= Time.deltaTime;
		else
		{
			display += display[0];
			display = display.Substring(1);

			timer = scrollSpeed / display.Length;
			text.text = display;
		}
	}


	public void UpdateDisplay(string song)
	{
		display = "now playing: " + song + "   ";
		timer = scrollSpeed / display.Length;
		text.text = display;
	}
}
