using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class LevelCaller : MonoBehaviour
{
    public int offSet = 0;
    public void CallScene()
    {
        Text sceneText = GetComponentInChildren<Text>();
        string sceneNumber = sceneText.text;
        SceneManager.LoadScene(int.Parse(sceneNumber) + (offSet - 1));
    }
}
