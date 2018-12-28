using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SpriteFader : MonoBehaviour
{

    public Image image;
    public float fadeRate = 10;

    public void DoFade()
    {
        if (image.color.a == 0)
            StartCoroutine(FadeIn());
        if (image.color.a == 1)
            StartCoroutine(FadeOut());

    }

    public IEnumerator FadeIn()
    {
        Color newColor = image.color;
        while (image.color.a <= 1)
        {
            newColor.a++;
            image.color = newColor;
            yield return new WaitForSeconds(1 / fadeRate);
        }
        newColor.a = 1;
        image.color = newColor;
		StopCoroutine(FadeIn());
    }
    public IEnumerator FadeOut()
    {
        Color newColor = image.color;
        while (image.color.a >= 0)
        {
            newColor.a--;
            image.color = newColor;
            yield return new WaitForSeconds(1 / fadeRate);
        }
        newColor.a = 0;
        image.color = newColor;
		StopCoroutine(FadeOut());
    }
}
