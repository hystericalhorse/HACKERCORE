using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
	string ALPHANUMERICS = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
	[SerializeField] TextMeshProUGUI title;
	[SerializeField] GameObject[] buttons;

	public void Start()
	{
		Time.timeScale = 1;

		title.fontSize = 160;
		StartCoroutine(AnimateTitle());
		foreach (var go in buttons)
		{
			go.SetActive(false);
		}
	}

	public IEnumerator AnimateTitle()
	{
		for (float i = 3; i > 0;)
		{
			title.fontSize = Mathf.Lerp(title.fontSize, 100, (title.fontSize / 100) * Time.deltaTime);

			char x = ALPHANUMERICS[UnityEngine.Random.Range(0, ALPHANUMERICS.Length - 1)];
			char y = ALPHANUMERICS[UnityEngine.Random.Range(0, ALPHANUMERICS.Length - 1)];
			char z = ALPHANUMERICS[UnityEngine.Random.Range(0, ALPHANUMERICS.Length - 1)];
			char w = ALPHANUMERICS[UnityEngine.Random.Range(0, ALPHANUMERICS.Length - 1)];
			title.text = $"H{x}CK{y}RC{z}R{w}";

			i -= Time.deltaTime;
			yield return new WaitForEndOfFrame();
		}

		title.text = "H4CK3RC0R3";

		foreach (var go in buttons)
		{
			yield return new WaitForSeconds(0.25f);
			go.SetActive(true);
		}
	}

	public void StartGame()
	{
		SceneManager.LoadScene("Game");
	}

	public void Options()
	{

	}

	public void QuitApplication()
	{

	}
}
