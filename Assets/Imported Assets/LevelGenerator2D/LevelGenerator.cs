using UnityEngine;
using System.Collections.Generic;

[ExecuteInEditMode]
public class LevelGenerator : MonoBehaviour
{
	public float correctionFactor = 1f;
	public bool generateAtStart = false;

	[Space]

	public Texture2D map;

	public ColorToPrefab[] colorMappings;
	private List<GameObject> instantiatedLevel = new List<GameObject>();

	private void Start()
	{
		if (generateAtStart)
		{
			GenerateLevel();
		}
	}

	public void GenerateLevel() {
		for (int x = 0; x < map.width; x++) {
			for (int y = 0; y < map.height; y++) {
				GenerateTile (x, y);
			}
		}
	}

	void GenerateTile (int x, int y) {
		Color pixelColor = map.GetPixel (x, y);
	
		if (pixelColor.a == 0)
			return;

		foreach (ColorToPrefab colorMapping in colorMappings) {
			if (colorMapping.color.Equals (pixelColor)) {
				Vector2 position = new Vector2(x * correctionFactor, y * correctionFactor);
				GameObject gamo = Instantiate (colorMapping.prefab, position, Quaternion.identity, transform);
				instantiatedLevel.Add(gamo);
			}
		}
	}

	public void DeleteLevel()
	{
		foreach (GameObject gamo in instantiatedLevel)
		{
			DestroyImmediate(gamo);
		}

		instantiatedLevel.Clear();

		SpriteRenderer[] gamos = gameObject.GetComponentsInChildren<SpriteRenderer>();
		foreach (SpriteRenderer gamo in gamos)
		{
			DestroyImmediate(gamo.gameObject);
		}
	}
}

/* Requisites for sprites
 * 
 * Compression = none
 * Filter mode = point (no filter)
 * Advanced > Can read/write
 */
