using UnityEngine;

public class AudioManagerSetter : MonoBehaviour
{
    public GameObject audioManagerPrefab;

    void Start()
    {
        GameObject audioManager = GameObject.Find("AudioManager");
        if (audioManager == null)
        {
            audioManager = Instantiate(audioManagerPrefab);
            audioManager.name = "AudioManager";

            audioManager.GetComponent<AudioManager>().Play("Theme");
        }
    }
}
