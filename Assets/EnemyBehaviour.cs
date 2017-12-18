using UnityEngine;

public class EnemyBehaviour : MonoBehaviour {
	
	private GameObject player;
	private Vector3 playerPos;

	private Animator anim;
	private float range = 0.5f;

	private GameObject attackObj;
	private GameObject walkObj;
	private GameObject idleObj;

	private Rigidbody2D rb;

	public float speed = 1f;

	void Start ()
	{
		player = GameObject.FindGameObjectWithTag("Player");

		attackObj = transform.Find("Skeleton Attack").gameObject;
		walkObj = transform.Find("Skeleton Walk").gameObject;
		idleObj = transform.Find("Skeleton Idle").gameObject;

		StopActing();

		rb = GetComponent<Rigidbody2D>();
	}

	void Update()
	{
		playerPos = player.GetComponent<Transform>().position;

	}

	void FixedUpdate()
	{
		if ((transform.position - playerPos).magnitude < 0.3f)// && !anim.GetBool("Attack"))
		{
			Attack();
			return;
		}

		Walk();
	}

	private void StopActing()
	{
		attackObj.SetActive(false);
		walkObj.SetActive(false);
		idleObj.SetActive(false);
	}

	private void Attack ()
	{
		//anim.SetBool("Attack", !anim.GetBool("Attack"));
		walkObj.SetActive(false);
		attackObj.SetActive(true);
	}

	private void Walk()
	{
		//anim.SetBool("Attack", !anim.GetBool("Attack"));
		attackObj.SetActive(false);
		walkObj.SetActive(true);

		int sentido;

		Vector3 relPos = (playerPos - transform.position);

		Flip(relPos.x < 0);

		if (relPos.x < 0)
		{
			sentido = -1;
		}
		else
			sentido = 1;

		transform.Translate(Vector3.right * speed * sentido * Time.fixedDeltaTime);
	}

	private void Flip(bool _bool)
	{
		attackObj.transform.GetComponentInChildren<SpriteRenderer>().flipX = _bool;
		walkObj.transform.GetComponentInChildren<SpriteRenderer>().flipX = _bool;
		idleObj.transform.GetComponentInChildren<SpriteRenderer>().flipX = _bool;

	}
}
