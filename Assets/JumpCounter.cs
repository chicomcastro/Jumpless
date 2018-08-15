using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JumpCounter : MonoBehaviour
{

    public int maxJumps;

    private int jumpCount;

    public Text jumpText;

    public static JumpCounter instance;
    
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    void Start()
    {
        jumpCount = maxJumps;
        jumpText.text = "x " + jumpCount.ToString();
    }

    void Update()
    {

        if (jumpCount < 0)
        {
            //Restart
            _SceneLoader.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
            return;
        }
    }

    public void DiscountJump()
    {
        jumpCount--;
        jumpText.text = "x " + jumpCount.ToString();
    }
}
