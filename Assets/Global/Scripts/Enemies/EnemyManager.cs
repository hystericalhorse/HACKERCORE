using UnityEngine;
using System;
using System.Collections.Generic;
using System.Collections;

public class EnemyManager : MonoBehaviour
{
	public int wave = 1;
	public int currentWaveCount = 0;
	public int maxWaveCount = 1;
	public List<EnemyInfo> enemiesRegistry = new();
	public List<GameObject> enemies = new();
	public Transform target;
	public GameObject parent;

	public void Awake()
	{
		if (enemiesRegistry.Count <= 0) Destroy(this);
	}

	public void Start()
	{
		if (target == null) target = GameObject.FindGameObjectWithTag("Player").transform;
		wave = 1;
		maxWaveCount = 5;
	}

	public void Update()
	{
		if (!GameManager.GM.isPlaying) return;

		if (enemies.Count <= 0 && currentWaveCount <= 0)
		{
			NewWave();
		}
	}

	public void LateUpdate()
	{
		currentWaveCount = enemies.Count;
	}

	public void NewWave()
	{
		currentWaveCount = UnityEngine.Random.Range(1, maxWaveCount);
		StartCoroutine(SpawnEnemies());

		maxWaveCount += 2;
		wave += 1;
	}

	public IEnumerator SpawnEnemies()
	{
		List<EnemyInfo> spawnableThisLevel = new();
		foreach (EnemyInfo e in enemiesRegistry)
		{
			if (GameManager.GM.Player.Level >= e.requiredMinLevel)
				spawnableThisLevel.Add(e);
		}

		for (int i = currentWaveCount; i > 0; i--)
		{
			Vector2 vec2 = UnityEngine.Random.insideUnitCircle.normalized * 30;

			while (!WithinBounds(ref vec2, 40, 40))
			{
				vec2 = UnityEngine.Random.insideUnitCircle.normalized * 30;
				vec2 += (Vector2)target.position;
			}

			var enemy = Instantiate(GetNewEnemy(spawnableThisLevel, currentWaveCount).prefab, vec2, Quaternion.identity, parent.transform);
			enemy.GetComponent<Enemy>().callOnDeath += () => {
				GameManager.GM.GiveXP(enemy.GetComponent<Enemy>().xpValue);
				GameManager.GM.Player.Kills++;
				enemies.Remove(enemy); 
			};
			enemies.Add(enemy);

			yield return new WaitForSeconds(UnityEngine.Random.Range(0.1f, 2f));
		}
	}

	public EnemyInfo GetNewEnemy(List<EnemyInfo> list, int count)
	{
		List<EnemyInfo> weightedList = new();
		foreach (EnemyInfo e in list)
		{
			for (int i = 0; i < (int)(count * e.weight); i++)
			{
				weightedList.Add(e);
			}
		}

		return weightedList[UnityEngine.Random.Range(0, weightedList.Count)];
	}

	private bool WithinBounds(ref Vector2 vec, float hBounds, float vBounds) => ((vec.x < hBounds && vec.x > -hBounds) && (vec.y < vBounds && vec.y > -vBounds));

	[Serializable]
	public class EnemyInfo
	{
		public float weight = 0.5f;
		public int requiredMinLevel = 0;
		public GameObject prefab;
	}
}