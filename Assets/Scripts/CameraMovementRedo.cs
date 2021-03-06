using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovementRedo : MonoBehaviour
{
	[SerializeField] Vector2 sensitivity;
	[SerializeField] float maxVerticalAngleFromHorizon;
	[SerializeField] float homingSpeedMultiplier;
	[SerializeField] GameObject[] apps = new GameObject[5]; // bosu, client, notes, mail, bamazon
	Vector2 rotation;
	bool isReleasing;
	bool isZeroed;
	Vector3 homingFrom;
	Vector3 homingTo;
	float FOVFrom;
	float FOVTarget;
	float t = 0;
	int currentFOV = 0;
	Vector2 GetInput()
	{
		Vector2 input = new Vector2
		(
			Input.GetAxis("Mouse X"),
			Input.GetAxis("Mouse Y")
		);
		return input;

	}

	float ClampVerticleAngle(float angle)
	{
		return Mathf.Clamp(angle, -maxVerticalAngleFromHorizon, maxVerticalAngleFromHorizon);
	}

	int SetViewMode()
	{
		if (apps[0].activeInHierarchy) // bosu
		{
			homingTo = new Vector3(0, 5, 0);
			FOVTarget = 50;
			return 1;
		}
		else if (apps[3].activeInHierarchy || apps[1].activeInHierarchy) // mail, bamazon, client
		{
			homingTo = new Vector3(0, 15, 0);
			FOVTarget = 40;
			return 2;
		}
		else if (apps[4].activeInHierarchy) // mail, bamazon, client
		{
			homingTo = new Vector3(0, 15, 0);
			FOVTarget = 70;
			return 2;
		}
		else // desktop
		{
			homingTo = Vector3.zero;
			FOVTarget = 55;
			return 0;
		}

		// 0 = desktop, 1 = bosu, 2 = mail bamazon client

	}



	// Start is called before the first frame update
	void Start()
	{
		homingFrom = new Vector3();
		homingTo = new Vector3();
		FOVFrom = GetComponent<Camera>().fieldOfView;
		FOVTarget = GetComponent<Camera>().fieldOfView;
	}

	// Update is called once per frame
	void Update()
	{


		//GetInitialRotation();
		if (Input.GetMouseButton(0))
		{
			
			if (!isReleasing || ApproximatelyEqual(transform.localEulerAngles, homingTo, 0.1f))
			{
				isReleasing = false;
				Vector2 wantedVelocity = GetInput() * sensitivity;

				rotation += wantedVelocity * Time.deltaTime; // replace with velocity if you want acceleration
				rotation.y = ClampVerticleAngle(rotation.y);
				transform.localEulerAngles = new Vector3(rotation.y, rotation.x, 0);
			}

		}
		else // resetting to 0,0,0
		{
			int tempFOV = SetViewMode();
			if (currentFOV != tempFOV)
			{
				isReleasing = false;
				currentFOV = tempFOV;
			}
			if (!isReleasing)
			{
				rotation.x = homingTo.y;
				rotation.y = homingTo.x;
				homingFrom = transform.localEulerAngles;
				FOVFrom = GetComponent<Camera>().fieldOfView;
				t = 0;
				isReleasing = true;
			}

			//rotation = new Vector2(transform.localEulerAngles.x, transform.localEulerAngles.y);
			transform.localEulerAngles = new Vector3
			(
				Mathf.LerpAngle(homingFrom.x, homingTo.x, t),
				Mathf.LerpAngle(homingFrom.y, homingTo.y, t),
				Mathf.LerpAngle(homingFrom.z, homingTo.z, t)
			);
			GetComponent<Camera>().fieldOfView = Mathf.Lerp(FOVFrom, FOVTarget, t);
			t += Time.deltaTime * homingSpeedMultiplier;



		}


		bool ApproximatelyEqual(Vector2 a, Vector2 b, float delta)
		{
			bool x = Math.Abs(a.x - b.x) < delta;
			bool y = Math.Abs(a.y - b.y) < delta;
			return x && y;
		}

	}
}
