using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class XPDisplay : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI text;
	[SerializeField] float f = 1;
	bool f_ = true;
	float timer;
	string display = string.Empty;
	int lastXP;

	public void Start()
	{
		timer = f;
	}

	public void Update()
	{
		timer -= Time.deltaTime;
		if (timer <= 0)
		{
			timer = f;
			f_ = !f_;
		}

		var pcent = (float)GameManager.GM.Player.XP / (float)GameManager.GM.Player.NextLevel;
		text.text = $"hack in progress {Mathf.CeilToInt(pcent * 100)}%" + display + ((f_) ? "_" : "");
	}

	public void UpdateXPDisplay()
	{
		if (GameManager.GM.Player.XP == 0) display = string.Empty;
		else StartCoroutine(UpdateDisplay());

		lastXP = GameManager.GM.Player.XP;
	}

	private IEnumerator UpdateDisplay()
	{
		var temp = display;
		var pcent = (float)(GameManager.GM.Player.XP - lastXP) / (float)GameManager.GM.Player.NextLevel;
		for (int i = Mathf.CeilToInt(pcent * 20); i > 0; i--)
		{
			temp += ".";
			display = temp;
			yield return new WaitForSeconds(0.2f);
		}
	}
}
