using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JumpCounter : MonoBehaviour
{

    public int maxJumps;

    private int jumpCount;
    private bool haveFinished = false;

    public Text jumpText;

    public static JumpCounter instance;

    private Image spacebarImage;

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
        jumpText.text = "x" + jumpCount.ToString();

        spacebarImage = GameObject.Find("Canvas").transform.Find("Panel").transform.Find("SpacebarImage").GetComponent<Image>();
    }

    void Update()
    {
        if (jumpCount == 0)
            haveFinished = true;

        if (jumpCount < 0)
        {
            // Don't enter here again
            jumpCount = 99;

            //Restart scene
            StartCoroutine(ResetScene());
            return;
        }
    }

    public void DiscountJump()
    {
        jumpCount--;

        if (haveFinished)
            return;

        jumpText.text = "x" + jumpCount.ToString();

        Color newColor = spacebarImage.color;
        newColor.g = (maxJumps - jumpCount) * 1.0f / maxJumps * (-1) + 1.0f;
        newColor.b = (maxJumps - jumpCount) * 1.0f / maxJumps * (-1) + 1.0f;
        spacebarImage.color = newColor;
    }

    public static IEnumerator ResetScene()
    {
        // Plays death song
        GameObject.Find("AudioManager").GetComponent<AudioManager>().Play("Death");
        
        // Search for fadeInImage
        Image image = GameObject.Find("Canvas").transform.Find("FadeInImage").GetComponent<Image>();
        image.gameObject.GetComponent<Animator>().enabled = false;
        image.enabled = true;

        // Setup fade in
        Color newColor = image.color;
        float fadePace = (Camera.main.orthographicSize - 0.5f) / 0.1f;
        float n = 0;

        // Starts acting (zoom in and fade in)
        while (Camera.main.orthographicSize >= 0.5f)
        {
            Camera.main.orthographicSize -= 0.1f;

            newColor.a = n/fadePace;
            image.color = newColor;
            n++;

            yield return new WaitForSeconds(0.01f);
        }
        _SceneLoader.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
    }
}
