using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireworkManager : MonoBehaviour
{

    public GameObject fireworkPrefab;

    void Start()
    {
        InvokeRepeating("InstantiateFirework", 1.0f, 1.0f);
        InvokeRepeating("InstantiateFirework", 1.5f, Random.Range(0.5f, 1.5f));

		GameObject.Find("AudioManager").GetComponent<AudioManager>().Play("Fireworks");
    }

    void InstantiateFirework()
    {
        GameObject gamo = Instantiate(fireworkPrefab, Random.insideUnitCircle * 7.5f + new Vector2(10.0f, 12.5f), Quaternion.identity);
        Destroy(gamo, 2.0f);
    }
}
