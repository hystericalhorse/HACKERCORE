using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HealthDisplay : MonoBehaviour
{
	[SerializeField] TextMeshProUGUI text;
	[SerializeField] Slider slider;

	public void UpdateHealthDisplay()
	{
		var health_as_binary = System.Convert.ToString(GameManager.GM.Player.Health, 2).PadLeft(6, '0');

		slider.value = (float) GameManager.GM.Player.Health / (float) GameManager.GM.Player.MaxHealth;
		text.text = health_as_binary;
	}
}
