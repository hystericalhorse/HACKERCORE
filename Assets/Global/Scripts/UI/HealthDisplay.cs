using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HealthDisplay : MonoBehaviour
{
	[SerializeField] TextMeshProUGUI text;

	public void UpdateHealthDisplay()
	{
		var health_as_binary = System.Convert.ToString(GameManager.GM.Player.Health, 2).PadLeft(8, '0');
		text.text = health_as_binary;
	}
}
