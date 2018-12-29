using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DataHolder : MonoBehaviour
{

    public static DataHolder instance;

    public int puzzleLevelReached, compLevelReached;
	public float musicVolume, effectsVolume;
    public GameType gameType;
    public Slider musicSlider, effectsSlider;

    void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }

        puzzleLevelReached = PlayerPrefs.GetInt("puzzleLevelReached", 1);
        compLevelReached = PlayerPrefs.GetInt("compLevelReached", 1);
ShowPlayerPrefsValues();
        musicVolume = PlayerPrefs.GetFloat("musicVolume");
        effectsVolume = PlayerPrefs.GetFloat("effectsVolume");
		// ISSO FAZ O EFFECTSVOLUME IR PRA 1 <<<<<
		// musicSlider.value = musicVolume;
		// effectsSlider.value = effectsVolume;
		ShowPlayerPrefsValues();
    }

    void Start()
    {
        AttSliderValue();
    }

    public void AttSliderValue() // Não tá sendo chamado no começo
    {
		musicVolume = musicSlider.value;
		effectsVolume = effectsSlider.value;

        PlayerPrefs.SetFloat("musicVolume", musicSlider.value);
        PlayerPrefs.SetFloat("effectsVolume", effectsSlider.value);
        PlayerPrefs.Save();

        AudioManager audioManager = GameObject.FindObjectOfType<AudioManager>();

        if (audioManager == null)
            print("AudioManager null");

        foreach (Sound s in audioManager.sounds)
        {
            foreach (AudioSource a in audioManager.gameObject.GetComponents<AudioSource>())
            {
                if (s.clip == a.clip)
                {
                    if (s.soundType == SoundType.Music)
                    {
                        a.volume = musicSlider.value;
                    }
                    if (s.soundType == SoundType.Effect)
                    {
                        a.volume = effectsSlider.value;
                    }
                }
            }
        }
    }

    [ContextMenu("Show level reached number")]
    public void ShowPlayerPrefsValues()
    {
        Debug.ClearDeveloperConsole();
        print(PlayerPrefs.GetInt("puzzleLevelReached"));
        print(PlayerPrefs.GetInt("compLevelReached"));
        print(PlayerPrefs.GetFloat("musicVolume"));
        print(PlayerPrefs.GetFloat("effectsVolume"));
    }

}
