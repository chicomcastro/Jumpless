using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour {

	[SerializeField]
	public List<EnemySpot> enemySpots = new List<EnemySpot>();

	private void Start()
	{
		InvokeRepeating("SpawnEnemy", 5.0f, 15.0f);
	}

	private void SpawnEnemy()
	{
		foreach(EnemySpot es in enemySpots)
		{
			Instantiate(es.enemy, es.spot.position, Quaternion.identity);
		}
	}
}

[System.Serializable]
public class EnemySpot
{
	public Transform spot;
	public GameObject enemy;
}
