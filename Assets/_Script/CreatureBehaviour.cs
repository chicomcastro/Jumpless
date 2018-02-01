using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class CreatureBehaviour : MonoBehaviour {
	
	private Idle idleState;
	private Patrol patrolState;
	private Run runState;

	private List<State> states = new List<State>();
	private State currentState;
	private LayerMask groundlayers;

	[HideInInspector]
	public bool finishedScan_L = false, finishedScan_R = false;

	private void Start()
	{
		// Set layers to analyse collisions
		groundlayers = GetComponent<PlatformerMotor2D>().staticEnvLayerMask;

		// Initializing two actual states
		idleState = new Idle();
		patrolState = new Patrol();
		runState = new Run();

		// Adding them to our state list
		states.Add(idleState);
		states.Add(patrolState);
		states.Add(runState);

		// Set states's gameobject reference, aka initialize (again...)
		InitializeStates();

		// Scan enviroment to set patrolState constrains
		patrolState.PrepareForPatroling();

		// Stay in idle officially
		EnterState(idleState);
	}

	private void InitializeStates()
	{
		foreach (State s in states)
		{
			s.creature = gameObject;
		}

		currentState = idleState;
	}

	private void EnterState(State _state)
	{
		currentState.ExitState();
		currentState = _state;
		_state.EnterState();
	}

	[ContextMenu("State: Idle")]
	private void Test_Idle()
	{
		EnterState(idleState);
	}

	[ContextMenu("State: Patrol")]
	private void Test_Patrol ()
	{
		EnterState(patrolState);
	}

	[ContextMenu("State: Run")]
	private void Test_Run()
	{
		EnterState(runState);
	}

	private void Update()
	{
		if(Input.GetKeyDown(KeyCode.Alpha1))
		{
			Test_Idle();
		}
		if (Input.GetKeyDown(KeyCode.Alpha2))
		{
			Test_Patrol();
		}
		if (Input.GetKeyDown(KeyCode.Alpha3))
		{
			Test_Run();
		}
	}

	public void StartCoroutineByName(string _coroutineName)
	{
		StartCoroutine(_coroutineName);
	}

	public void StopCoroutineByName(string _coroutineName)
	{
		StopCoroutine(_coroutineName);
	}

	IEnumerator Patrol()
	{
		yield return new WaitUntil(() => finishedScan_L && finishedScan_R);
		finishedScan_L = false;
		finishedScan_R = false;
		print("I'm patroling");

		while (patrolState.actived)
		{
			gameObject.GetComponent<IACreatureController2D>().SimulateButton("RIGHT");
			yield return new WaitUntil(() => transform.position.x >= patrolState.a_right || patrolState.actived == false);
			gameObject.GetComponent<IACreatureController2D>().SimulateButton("LEFT");
			yield return new WaitUntil(() => transform.position.x <= patrolState.a_left || patrolState.actived == false);
		}

		gameObject.GetComponent<IACreatureController2D>().SimulateButton("RELEASE");
		StopCoroutine(Patrol());
	}

	IEnumerator ScanEnviromentForRight()
	{
		Vector2 initialFeetPos;

		Ray2D ray = new Ray2D();
		ray.origin = transform.position + new Vector3(0, gameObject.GetComponent<Collider2D>().bounds.extents.y, 0);

		RaycastHit2D hit_right = Physics2D.Raycast(ray.origin, new Vector2(1, 0), Mathf.Infinity, groundlayers.value);
		patrolState.a_right = hit_right.point.x - GetComponent<Collider2D>().bounds.size.x;

		RaycastHit2D raycastHit2D_initial = Physics2D.Raycast(ray.origin, new Vector2(0, -1), Mathf.Infinity, groundlayers.value);
		initialFeetPos = raycastHit2D_initial.point;
		
		for (float x = initialFeetPos.x; x <= patrolState.a_right; x += 0.1f)
		{
			RaycastHit2D hit = Physics2D.Raycast(new Vector2(x, ray.origin.y), new Vector2(0, -1), Mathf.Infinity, groundlayers.value);
			
			if (Mathf.Abs(hit.point.y - initialFeetPos.y) > GetComponent<Collider2D>().bounds.size.y)
			{
				patrolState.a_right = hit.point.x - GetComponent<Collider2D>().bounds.size.x;
				break;
			}
		}

		yield return null;

		finishedScan_R = true;

		StopCoroutine(ScanEnviromentForRight());
	}

	IEnumerator ScanEnviromentForLeft()
	{
		Vector2 initialFeetPos;

		Ray2D ray = new Ray2D();
		ray.origin = transform.position + new Vector3(0, gameObject.GetComponent<Collider2D>().bounds.extents.y, 0);

		RaycastHit2D hit_left = Physics2D.Raycast(ray.origin, new Vector2(-1, 0), Mathf.Infinity, groundlayers.value);
		patrolState.a_left = hit_left.point.x + GetComponent<Collider2D>().bounds.size.x;

		RaycastHit2D raycastHit2D_initial = Physics2D.Raycast(ray.origin, new Vector2(0, -1), Mathf.Infinity, groundlayers.value);
		initialFeetPos = raycastHit2D_initial.point;
		
		for (float x = initialFeetPos.x; x >= patrolState.a_left; x -= 0.1f)
		{
			RaycastHit2D hit = Physics2D.Raycast(new Vector2(x, ray.origin.y), new Vector2(0, -1), Mathf.Infinity, groundlayers.value);
			
			if (Mathf.Abs(hit.point.y - initialFeetPos.y) > GetComponent<Collider2D>().bounds.size.y)
			{
				patrolState.a_left = hit.point.x + GetComponent<Collider2D>().bounds.size.x;
				break;
			}
		}

		yield return null;

		finishedScan_L = true;

		StopCoroutine(ScanEnviromentForLeft());
	}

	IEnumerator RunAndJump()
	{
		GetComponent<IACreatureController2D>().enabled = false;
		while (runState.actived)
		{
			runState._motor.normalizedXMovement = runState.movement;

			if (runState._motor.motorState == PlatformerMotor2D.MotorState.OnGround)
			{
				runState._motor.fallFast = false;
				Vector2 dir = Vector2.right;

				if (runState.movement < 0)
				{
					dir *= -1;
				}

				RaycastHit2D hit = Physics2D.Raycast(
					transform.position,
					dir,
					runState.distanceCheckForJump,
					PC2D.Globals.ENV_MASK);

				if (hit.collider != null)
				{
					runState._motor.Jump();
				}
			}

			if (runState._motor.motorState == PlatformerMotor2D.MotorState.WallSticking)
			{
				StartCoroutine(DelayWallJump(Random.Range(0.5f, 2f)));
			}

			if (runState._motor.motorState == PlatformerMotor2D.MotorState.OnCorner)
			{
				StartCoroutine(DelayWallJump(Random.Range(1f, 2f)));
			}

			if (runState._motor.motorState == PlatformerMotor2D.MotorState.Falling)
			{
				RaycastHit2D hit = Physics2D.Raycast(
					transform.position,
					-Vector2.up,
					runState.heightToFallFast,
					PC2D.Globals.ENV_MASK);

				Vector2 dir = Vector2.right;

				if (runState.movement < 0)
				{
					dir *= -1;
				}

				RaycastHit2D hit2 = Physics2D.Raycast(
					transform.position,
					dir,
					runState.distanceCheckForJump,
					PC2D.Globals.ENV_MASK);

				if (hit.collider == null && hit2.collider == null)
				{
					runState._motor.fallFast = true;
				}
			}

			yield return new WaitForFixedUpdate();
		}

		GetComponent<IACreatureController2D>().enabled = true;
		StopCoroutine(RunAndJump());
	}

	private IEnumerator DelayWallJump(float delayForWallJump)
	{
		yield return new WaitForSeconds(delayForWallJump);
		runState._motor.Jump();
		StopCoroutine("DelayWallJump");
	}
}

public class State
{
	public bool actived = false;
	[HideInInspector]
	public GameObject creature = null;

	public virtual void EnterState ()
	{
		actived = true;
	}

	public virtual void ExitState()
	{
		actived = false;
	}
}

[System.Serializable]
public class Idle : State
{
	public override void EnterState()
	{
		base.EnterState();

		Debug.Log("I'm idling");
	}
}

[System.Serializable]
public class Patrol : State
{
	[Header("Patroling parameters")]
	public float a_left;
	public float a_right;

	public void PrepareForPatroling()
	{
		if (creature == null)
		{
			Debug.Log("Creature from patrol state is null");
			return;
		}
	}

	public override void EnterState()
	{
		base.EnterState();

		creature.GetComponent<CreatureBehaviour>().StartCoroutineByName("ScanEnviromentForLeft");
		creature.GetComponent<CreatureBehaviour>().StartCoroutineByName("ScanEnviromentForRight");
		creature.GetComponent<CreatureBehaviour>().StartCoroutineByName("Patrol");
	}
}

[System.Serializable]
public class Run : State
{
	[Header("Jumping parameters")]
	public float distanceCheckForJump = 2f;
	public float heightToFallFast = 4f;
	public float delayForWallJump = 0.1f;

	public PlatformerMotor2D _motor;

	[HideInInspector]
	public float movement { get; private set; }

	public override void EnterState()
	{
		base.EnterState();

		_motor = creature.gameObject.GetComponent<PlatformerMotor2D>();
		movement = -1;

		// Find objects generally pretty bad but this is a demo :)
		PC2D.SimpleAI[] ais = Object.FindObjectsOfType<PC2D.SimpleAI>();

		for (int i = 0; i < ais.Length; i++)
		{
			Physics2D.IgnoreCollision(creature.gameObject.GetComponent<Collider2D>(), ais[i].GetComponent<Collider2D>());
		}

		_motor.onWallJump +=
			dir =>
			{
				// Since the motor needs to be pressing into the wall to wall jump, we switch direction after the jump.

				movement = Mathf.Sign(dir.x);
			};

		creature.gameObject.GetComponent<CreatureBehaviour>().StartCoroutineByName("RunAndJump");

		Debug.Log("I'm running");
	}
}