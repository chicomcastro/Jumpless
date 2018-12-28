using UnityEngine;
using UnityEngine.UI;

public class SpriteDisabler : MonoBehaviour {

	public Image[] images;

	public void ManageImage() {
		foreach (Image image in images)
		{
			image.enabled = !image.enabled;
		}
	}
}
