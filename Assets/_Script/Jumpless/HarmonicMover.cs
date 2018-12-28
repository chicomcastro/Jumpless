using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HarmonicMover : MonoBehaviour
{
    public Vector2 amplitude;
    public Vector2 frequency;

	private Vector3 originalPos;

	void Start()
	{
		originalPos = transform.position;
	}

    void Update()
    {
        transform.position = originalPos 
		+ amplitude.x * Mathf.Sin(frequency.x * Time.time) * Vector3.right
		+ amplitude.y * Mathf.Sin(frequency.y * Time.time) * Vector3.up;
	}
}
