using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class _SceneLoader : MonoBehaviour {

	public string sceneToCall;

	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.tag == "Player") {
			UnityEngine.SceneManagement.SceneManager.LoadScene(sceneToCall);
			GameObject.Find("AudioManager").GetComponent<AudioManager>().Play("NewLevel");
		}
	}

	public static void LoadScene(string sceneToLoad) {
		
			UnityEngine.SceneManagement.SceneManager.LoadScene(sceneToLoad);
	}

	public void LoadScene() {
		
			UnityEngine.SceneManagement.SceneManager.LoadScene(sceneToCall);
	}
}
