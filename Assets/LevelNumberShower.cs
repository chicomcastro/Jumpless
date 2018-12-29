using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelNumberShower : MonoBehaviour
{

    Text levelNumber;

    public int offSet;

    private void Start()
    {
        levelNumber = GetComponentInChildren<Text>();
        levelNumber.text = (SceneManager.GetActiveScene().buildIndex - (offSet + 1)).ToString();

        if (DataHolder.instance == null)
            return;

        GameType gameType = DataHolder.instance.gameType;
        if (gameType == GameType.Puzzle)
        {
            PlayerPrefs.SetInt("puzzleLevelReached", (SceneManager.GetActiveScene().buildIndex - (offSet + 1)));
            PlayerPrefs.Save();
        }
        if (gameType == GameType.Competition)
        {
            PlayerPrefs.SetInt("compLevelReached", (SceneManager.GetActiveScene().buildIndex - (offSet + 1)));
            PlayerPrefs.Save();
        }
    }

}
