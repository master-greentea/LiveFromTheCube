using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{

	public Transform target;
	public Transform focusTarget;
	public CatchPlayer sus;

	public float targetHeight = 1.7f;
	public float distance = 5.0f;
	public float offsetFromWall = 0.1f;

	public Camera mainCam;

	public float maxDistance = 20;
	public float minDistance = .6f;
	public float speedDistance = 5;

	private float baseZoom;
	public float maxZoom = 20;
	public float zoomToTargetSpeed = .5f;
	//public float zoomDuration = 20;

	public float xSpeed = 200.0f;
	public float ySpeed = 200.0f;

	public int yMinLimit = -40;
	public int yMaxLimit = 80;

	public int zoomRate = 40;

	public float rotationDampening = 3.0f;
	public float zoomDampening = 5.0f;

	public LayerMask collisionLayers = -1;

	private float xDeg = 0.0f;
	private float yDeg = 0.0f;
	private float currentDistance;
	private float desiredDistance;
	private float correctedDistance;

	private float screenHeight;
	private float screenWidth;
	public float speed = 5;
	public float boundary = 10;

	public float angleBoundpos;
	public float angleBoundneg; 

	public bool start = true;
	private bool cameraEdge = true;
	private bool cameraEdgeOnce = false; 



	public float MIN_X;
	public float MAX_X;
	public float MIN_Y;
	public float MAX_Y;
	public float MIN_Z;
	public float MAX_Z;

	void Start()
	{
		Vector3 angles = transform.eulerAngles;
		xDeg = angles.x;
		yDeg = angles.y;

		currentDistance = distance;
		desiredDistance = distance;
		correctedDistance = distance;

		baseZoom = Camera.main.fieldOfView;
		// Make the rigid body not change rotation
		if (this.gameObject.GetComponent<Rigidbody>())
			this.gameObject.GetComponent<Rigidbody>().freezeRotation = true;

		screenWidth = Screen.width;
		screenHeight = Screen.height;

	}

	void Update() {

		if(cameraEdge == true) {
			Cursor.visible = false;
			Quaternion rotation = Quaternion.Euler(yDeg, xDeg, 0);

			/*
			 * interesting effect #1 rotation.y = Mathf.Clamp(rotation.x, angleBoundneg, angleBoundpos);
			 * if this was implemented would remove the mouse rotation 
			 */

			rotation.x = Mathf.Clamp(rotation.y, angleBoundneg, angleBoundpos);
			rotation.y = Mathf.Clamp(rotation.y, angleBoundneg, angleBoundpos);
			
			xDeg += Input.GetAxis("Mouse X") * xSpeed * 0.02f;
			yDeg -= Input.GetAxis("Mouse Y") * ySpeed * 0.02f;

			rotation = Quaternion.Euler(yDeg, xDeg, 0);
			transform.rotation = rotation; 

			
		}
		if (Input.GetKeyDown(KeyCode.X) && cameraEdgeOnce == true) {
			Debug.Log(" edge on");
			cameraEdge = true;
			cameraEdgeOnce = false;

		}else if (Input.GetKeyDown(KeyCode.X)) {
			Debug.Log(" edge off");
			cameraEdge = false;
			cameraEdgeOnce = true;

		}
		if (Input.GetKey(KeyCode.Z)) {
			transform.rotation = Quaternion.identity;
		}



		// clamping
		// transform.rotation = new Vector3(
		// 	Mathf.Clamp(transform.rotation.x, MIN_X, MAX_X),
		// 	Mathf.Clamp(transform.rotation.y, MIN_Y, MAX_Y),
		// 	Mathf.Clamp(transform.rotation.z, MIN_Z, MAX_Z));
	}

	// Camera logic on LateUpdate to only update after all character movement logic has been handled.

	void LateUpdate(){
		Vector3 vTargetOffset;

		Quaternion rotation = Quaternion.Euler(yDeg, xDeg, 0);
		//rotation.y = Mathf.Clamp(rotation.y, angleBoundneg, angleBoundpos);

		if (!target)
			return;

		// If either mouse buttons are down, let the mouse govern camera position
		if (GUIUtility.hotControl == 0) {
			if (cameraEdge == false) {
				Cursor.visible = true;
				if (Input.GetMouseButton(0) || Input.GetMouseButton(1)) {
					xDeg += Input.GetAxis("Mouse X") * xSpeed * 0.02f;
					yDeg -= Input.GetAxis("Mouse Y") * ySpeed * 0.02f;

					rotation = Quaternion.Euler(yDeg, xDeg, 0);

					var pos = transform.position;
					pos.x = Mathf.Clamp(transform.position.x, angleBoundneg, angleBoundpos);
					transform.position = pos;

					transform.rotation = rotation;

				}
			}
		}

		correctedDistance = desiredDistance;

		// calculate desired camera position
		vTargetOffset = new Vector3(0, -targetHeight, 0);
		Vector3 position = target.position - (rotation * Vector3.forward * desiredDistance + vTargetOffset);

		// check for collision using the true target's desired registration point as set by user using height
		RaycastHit collisionHit;
		Vector3 trueTargetPosition = new Vector3(target.position.x, target.position.y, target.position.z) - vTargetOffset;

		// if there was a collision, correct the camera position and calculate the corrected distance
		bool isCorrected = false;
		if (Physics.Linecast(trueTargetPosition, position, out collisionHit, collisionLayers.value)) {

			// calculate the distance from the original estimated position to the collision location,
			// subtracting out a safety "offset" distance from the object we hit.  The offset will help
			// keep the camera from being right on top of the surface we hit, which usually shows up as
			// the surface geometry getting partially clipped by the camera's front clipping plane.
			correctedDistance = Vector3.Distance(trueTargetPosition, collisionHit.point) - offsetFromWall;
			isCorrected = true;
		}

		// For smoothing, lerp distance only if either distance wasn't corrected, or correctedDistance is more than currentDistance
		currentDistance = !isCorrected || correctedDistance > currentDistance ? Mathf.Lerp(currentDistance, correctedDistance, Time.deltaTime * zoomDampening) : correctedDistance;

		// keep within legal limits
		currentDistance = Mathf.Clamp(currentDistance, minDistance, maxDistance);
	}
}
