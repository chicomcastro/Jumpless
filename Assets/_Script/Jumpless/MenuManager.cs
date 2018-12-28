using UnityEngine;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{

    public GameObject menuObj;
	
    void Awake()
    {
        menuObj.SetActive(false);
    }

    public void CallMenu()
    {
        //Time.timeScale = 0;
        menuObj.SetActive(true);
    }

}
