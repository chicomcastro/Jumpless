using UnityEngine;

public class BulletMover : MonoBehaviour {

	public float speed = 1.0f;

	private void FixedUpdate()
	{
		transform.Translate(speed * Vector3.right * Time.fixedDeltaTime);
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.tag == "Enemy")
		{
			Destroy(other.gameObject);
			Destroy(this.gameObject);
		}

		if (other.gameObject.layer == 8)
			Destroy(this.gameObject);
	}
}
