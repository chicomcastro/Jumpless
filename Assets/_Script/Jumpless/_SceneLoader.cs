using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class _SceneLoader : MonoBehaviour
{

    public string sceneToCall;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            int validationByJumpCount = GameObject.FindObjectOfType<JumpCounter>().jumpCount;

            if (validationByJumpCount >= 0 && validationByJumpCount != 99)
            {
                UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex + 1);
                GameObject.Find("AudioManager").GetComponent<AudioManager>().Play("NewLevel");
            }
        }
    }

    public string GetNextSceneName() // NOT IN USE
    {
        string nextScene = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
        int sceneNumber = int.Parse(nextScene[nextScene.Length - 1].ToString());

        nextScene = new String(nextScene.Where(c => (c < '0' || c > '9')).ToArray());
        print(nextScene);

        return nextScene.Insert(nextScene.Length, (sceneNumber + 1).ToString());
    }

    public static void LoadScene(string sceneToLoad)
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(sceneToLoad);
    }

    public void LoadScene()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(sceneToCall);
    }
}
