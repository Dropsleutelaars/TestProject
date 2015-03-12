using UnityEngine;
using System.Collections;

public class ParallaxMark : MonoBehaviour {

    public Transform[] parallaxingObjects;      // Array with all back- and foreground to be parallaxed
    private float[] parallaxScales;             // The proportion of the camera's movement to move the background by
    public float smoothing = 1f;                // How smooth the parallax is going to be. Make sure to set this above 0.

    private Transform mainCamera;               // reference to the main camera transform
    private Vector3 previousCamPos;             // the position of the camera in the previous frame.

    // Use this to call logic before Start(). Great for references.
    void Awake ()
    {
        // Set up the Camera reference
        mainCamera = Camera.main.transform;
    }

	// Use this for initialization
	void Start () 
    {
	    // The Previous frame had the current frame's camera position
        previousCamPos = mainCamera.position;

        // assigning coresponding parallaxScales
        parallaxScales = new float[parallaxingObjects.Length];

        for (int i = 0; i < parallaxingObjects.Length; i++)
        {
            parallaxScales[i] = parallaxingObjects[i].position.z * -1;
        }
	}
	
	// Update is called once per frame
	void Update () 
    {
	    // for each background
        for (int i = 0; i < parallaxingObjects.Length; i++)
        {
            // the parallax is the opposite of the camera movement because the previous frame multiplied by the scale.
            float parallax = (previousCamPos.x - mainCamera.position.x) * parallaxScales[i];

            // set a target x position which is the current position + the parallax.
            float parallaxingObjectsTargetPosX = parallaxingObjects[i].position.x + parallax;

            // create a target position which is the background's current position with it's target x position.
            Vector3 parallaxingObjectsTargetPos = new Vector3(parallaxingObjectsTargetPosX, parallaxingObjects[i].position.y, parallaxingObjects[i].position.z);

            // fade between the current and the target position using lerp.
            parallaxingObjects[i].position = Vector3.Lerp(parallaxingObjects[i].position, parallaxingObjectsTargetPos, smoothing * Time.deltaTime);
        }

        // set the previousCamPos to the camera's position at the end of the frame.
        previousCamPos = mainCamera.position;
	}
}
