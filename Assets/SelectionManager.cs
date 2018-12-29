using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectionManager : MonoBehaviour
{

    public GameType gameType;
    public Button[] levelButtons;

    private int buttonsToActivate;

    private void Start()
    {
        if (DataHolder.instance == null)
            return;

        FindObjectOfType<DataHolder>().gameType = gameType;

        if (gameType == GameType.Puzzle)
        {
            buttonsToActivate = GameObject.Find("DataHolder").GetComponent<DataHolder>().puzzleLevelReached;
        }
        if (gameType == GameType.Competition)
        {
            buttonsToActivate = GameObject.Find("DataHolder").GetComponent<DataHolder>().compLevelReached;
        }

        for (int i = buttonsToActivate; i < levelButtons.Length; i++)
        {
            levelButtons[i].interactable = false;
        }
    }
}

public enum GameType { Puzzle, Competition };

