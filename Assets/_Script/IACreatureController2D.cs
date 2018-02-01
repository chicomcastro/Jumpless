using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlatformerMotor2D))]
public class IACreatureController2D : MonoBehaviour {

	private PlatformerMotor2D _motor;
	private bool _restored = true;
	private bool _enableOneWayPlatforms;
	private bool _oneWayPlatformsAreWalls;

	private bool haveJumped, dash, attack;
	private float horizontal = 0f, vertical = 0f;

	// Use this for initialization
	void Start()
	{
		_motor = GetComponent<PlatformerMotor2D>();
	}

	// before enter en freedom state for ladders
	void FreedomStateSave(PlatformerMotor2D motor)
	{
		if (!_restored) // do not enter twice
			return;

		_restored = false;
		_enableOneWayPlatforms = _motor.enableOneWayPlatforms;
		_oneWayPlatformsAreWalls = _motor.oneWayPlatformsAreWalls;
	}
	// after leave freedom state for ladders
	void FreedomStateRestore(PlatformerMotor2D motor)
	{
		if (_restored) // do not enter twice
			return;

		_restored = true;
		_motor.enableOneWayPlatforms = _enableOneWayPlatforms;
		_motor.oneWayPlatformsAreWalls = _oneWayPlatformsAreWalls;
	}

	// Update is called once per frame
	void Update()
	{
		// use last state to restore some ladder specific values
		if (_motor.motorState != PlatformerMotor2D.MotorState.FreedomState)
		{
			// try to restore, sometimes states are a bit messy because change too much in one frame
			FreedomStateRestore(_motor);
		}

		// Jump?
		// If you want to jump in ladders, leave it here, otherwise move it down
		if (haveJumped) //Input.GetButtonDown(PC2D.Input.JUMP))
		{
			haveJumped = false;
			_motor.Jump();
			_motor.DisableRestrictedArea();
		}

		_motor.jumpingHeld = true; // jumping; // Input.GetButton(PC2D.Input.JUMP);

		// XY freedom movement
		if (_motor.motorState == PlatformerMotor2D.MotorState.FreedomState)
		{
			_motor.normalizedXMovement = horizontal; //Input.GetAxis(PC2D.Input.HORIZONTAL);
			_motor.normalizedYMovement = vertical; //Input.GetAxis(PC2D.Input.VERTICAL);

			return; // do nothing more
		}

		// X axis movement
		if (Mathf.Abs(horizontal) > PC2D.Globals.INPUT_THRESHOLD)
		{
			_motor.normalizedXMovement = horizontal; //Input.GetAxis(PC2D.Input.HORIZONTAL);
		}
		else
		{
			_motor.normalizedXMovement = 0;
		}

		// Y axis movement
		if (vertical != 0)//Input.GetAxis(PC2D.Input.VERTICAL) != 0)
		{
			bool up_pressed = vertical > 0; //Input.GetAxis(PC2D.Input.VERTICAL) > 0;
			if (_motor.IsOnLadder())
			{
				if (
					(up_pressed && _motor.ladderZone == PlatformerMotor2D.LadderZone.Top)
					||
					(!up_pressed && _motor.ladderZone == PlatformerMotor2D.LadderZone.Bottom)
				 )
				{
					// do nothing!
				}
				// if player hit up, while on the top do not enter in freeMode or a nasty short jump occurs
				else
				{
					// example ladder behaviour

					_motor.FreedomStateEnter(); // enter freedomState to disable gravity
					_motor.EnableRestrictedArea();  // movements is retricted to a specific sprite bounds

					// now disable OWP completely in a "trasactional way"
					FreedomStateSave(_motor);
					_motor.enableOneWayPlatforms = false;
					_motor.oneWayPlatformsAreWalls = false;

					// start XY movement
					_motor.normalizedXMovement = horizontal; //Input.GetAxis(PC2D.Input.HORIZONTAL);
					_motor.normalizedYMovement = vertical; //Input.GetAxis(PC2D.Input.VERTICAL);
				}
			}
		}
		else if (vertical < -PC2D.Globals.FAST_FALL_THRESHOLD)
		{
			_motor.fallFast = false;
		}

		if (dash)
		{
			dash = false;
			_motor.Dash();
		}

		if (attack)
		{
			attack = false;
			gameObject.GetComponent<PC2D.PlatformerAnimation2D>().Attack();
		}
	}
	
	public void SimulateButton(string _button)
	{
		switch (_button)
		{
			case "JUMP":
				Jump();
				break;
			case "RIGHT":
				horizontal = 1.0f;
				break;
			case "LEFT":
				horizontal = -1.0f;
				break;
			case "DASH":
				Dash();
				break;
			case "ATTACK":
				Attack();
				break;
			default:
				horizontal = 0f;
				break;
		}
	}

	[ContextMenu("Jump")]
	public void Jump()
	{
		haveJumped = true;
	}

	[ContextMenu("Dash")]
	public void Dash()
	{
		dash = true;
	}

	[ContextMenu("Attack")]
	public void Attack()
	{
		attack = true;
	}
}
