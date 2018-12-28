using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonManagement : MonoBehaviour {

	public GameObject[] buttonsToEnable;
	
	public GameObject[] buttonsToDisable;

	public void DoTheThing() {
		foreach (GameObject gamo in buttonsToDisable)
		{
			gamo.SetActive(false);
		}
		foreach (GameObject gamo in buttonsToEnable)
		{
			gamo.SetActive(true);
		}
	}
}
