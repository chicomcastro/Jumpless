using System;
using UnityEngine;


namespace UnityStandardAssets._2D
{
    public class CameraFollow : MonoBehaviour
    {
        public float xMargin = 1f; // Distance in the x axis the player can move before the camera follows.
        public float yMargin = 1f; // Distance in the y axis the player can move before the camera follows.
        public float xSmooth = 8f; // How smoothly the camera catches up with it's target movement in the x axis.
        public float ySmooth = 8f; // How smoothly the camera catches up with it's target movement in the y axis.
        public Vector2 maxXAndY; // The maximum x and y coordinates the camera can have.
        public Vector2 minXAndY; // The minimum x and y coordinates the camera can have.

        public Transform m_Player; // Reference to the player's transform.

		public Transform[] backgrounds; //Arrary (list) of all the back and foregrounds to be parrallaxed 
		public float smoothing = 1f; // how smooth the parallax is going to be. Make sure to set this above 0 
		private float[] parallaxScales; // The proportion of the camera's movement to move the backgrounds by 
		private Transform cam; //reference to the main cameras transform 
		private Vector3 previousCamPos; //the position of the camera in teh previous frame 

		//Is called before Start(). Great for references 
		void Awake()
		{
			//set up camera reference 
			cam = transform;
		}

		// Use this for initialization 
		void Start()
		{
			//The previous fram the current fram's camera position 
			previousCamPos = cam.position;

			//asigning coresponding parallaxScales 
			parallaxScales = new float[backgrounds.Length];

			for (int i = 0; i < backgrounds.Length; i++)
			{
				parallaxScales[i] = backgrounds[i].position.z * -1;
			}

			InvokeRepeating("TrackCamera", 0.5f, 0.05f);
		}

        private void LateUpdate()
		{

			TrackPlayer();

		}
		
		private void TrackPlayer()
        {
            // By default the target x and y coordinates of the camera are it's current x and y coordinates.
            float targetX = transform.position.x;
            float targetY = transform.position.y;

            // If the player has moved beyond the x margin...
            if (CheckXMargin())
            {
                // ... the target x coordinate should be a Lerp between the camera's current x position and the player's current x position.
                targetX = Mathf.Lerp(transform.position.x, m_Player.position.x, xSmooth*Time.deltaTime);
            }

            // If the player has moved beyond the y margin...
            if (CheckYMargin())
            {
                // ... the target y coordinate should be a Lerp between the camera's current y position and the player's current y position.
                targetY = Mathf.Lerp(transform.position.y, m_Player.position.y, ySmooth*Time.deltaTime);
            }

            // The target x and y coordinates should not be larger than the maximum or smaller than the minimum.
            targetX = Mathf.Clamp(targetX, minXAndY.x, maxXAndY.x);
            targetY = Mathf.Clamp(targetY, minXAndY.y, maxXAndY.y);

            // Set the camera's position to the target position with the same z component.
            transform.position = new Vector3(targetX, targetY, transform.position.z);
		}

		private bool CheckXMargin()
		{
			// Returns true if the distance between the camera and the player in the x axis is greater than the x margin.
			return Mathf.Abs(transform.position.x - m_Player.position.x) > xMargin;
		}

		private bool CheckYMargin()
		{
			// Returns true if the distance between the camera and the player in the y axis is greater than the y margin.
			return Mathf.Abs(transform.position.y - m_Player.position.y) > yMargin;
		}

		private void TrackCamera()
		{
			//for each background 
			for (int i = 0; i < backgrounds.Length; i++)
			{
				// the parallax is the opposite of the camera  movement becuase teh previous frame multiplied bye the scale 
				Vector2 parallax = new Vector2( (previousCamPos.x - cam.position.x) * parallaxScales[i], (previousCamPos.y - cam.position.y) * parallaxScales[i]);

				// set a target x position which is the current position plus the parallax 
				float backgroundTargetPosX = backgrounds[i].position.x + parallax.x;
				float backgroundTargetPosY = backgrounds[i].position.y + parallax.y;

				// create a target position which is the background's current position with it's target x position 
				Vector3 backgroundTargetPos = new Vector3(backgroundTargetPosX, backgroundTargetPosY, backgrounds[i].position.z);

				// fade between current position and the target position using lerp 
				backgrounds[i].position = Vector3.LerpUnclamped(backgrounds[i].position, backgroundTargetPos, smoothing * Time.deltaTime);
			}

			// set the priviousCamPos to teh camera's position at the end of the frame  
			previousCamPos = cam.position;
		}
	}
}
