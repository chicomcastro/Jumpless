using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class LevelCaller : MonoBehaviour
{
    public void CallScene()
    {
        Text sceneText = GetComponentInChildren<Text>();
        string sceneNumber = sceneText.text;
        SceneManager.LoadScene("puzzle-level" + sceneNumber);
    }
}
