using UnityEngine;
using System.Collections;

public class ParallaxingDavid : MonoBehaviour {


	public Transform[] backgrounds;				// this an array (meaning a list) of all back- and forgrounds to be parallaxed
	private float[] parallaxedScales; 			// this is the porportion of the cameras movement to move the background by
	public float smoothing = 1f; 				    // this is how smooth the parallaxing is going to be. Make sure to set this above 0.

	private Transform cam; 						// reference to the main cameras transform.
	private Vector3 previousCamPos; 			// the position of the camera in the previous frame

	// is called before start(). great for references 
	void Awake () {
		// set up cam the reference
		cam = Camera.main.transform;


	} 

	// Use this for initialization
	void Start () {
		// the previous frame had the current cameras position
		previousCamPos = cam.position;

		// assigning corresponding parallaxScales
		parallaxedScales = new float[backgrounds.Length];

		for (int i = 0; i < backgrounds.Length; i++) {
			parallaxedScales [i] = backgrounds [i].position.z * -1;
			
		}
	}
	// Update is called once per frame
	void Update () {

			// for each background 
			for (int i = 0; i < backgrounds.Length; i++) {
				// the parallax is the opposite of the camera movement because the previous frame multiplied by the scale
				float parallax = (previousCamPos.x - cam.position.x) * parallaxedScales[i];

				// set a target x position which is the current position plus the parallax

				float backgroundTargetPosX = backgrounds[i].position.x + parallax;

				// creat a target position which is the backgrounds current psition with its target x position
				Vector3 backgroundTargetPos = new Vector3 (backgroundTargetPosX, backgrounds[i].position.y, backgrounds[i].position.z);

				// fade between current position and target position using lerp
				backgrounds[i].position = Vector3.Lerp (backgrounds[i].position, backgroundTargetPos, smoothing * Time.deltaTime);
			
			}
			// set the previousCamPos to the cameras position at the end of the gram
			previousCamPos = cam.position;
	
	}
}
