using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovementRedo : MonoBehaviour
{
	[SerializeField] Vector2 sensitivity;
	Vector2 rotation;

	Vector2 GetInput()
	{
		Vector2 input = new Vector2
		(
			Input.GetAxis("Mouse Y"),
			Input.GetAxis("Mouse X")
		);
		return input;

	}






	// Start is called before the first frame update
	void Start()
	{

	}

	// Update is called once per frame
	void Update()
	{
		if (Input.GetMouseButton(0))
		{
			Vector2 wantedVelocity = GetInput() * sensitivity;
			rotation += wantedVelocity * Time.deltaTime;
			transform.localEulerAngles = new Vector3(rotation.x, rotation.y, 0);
		}
		
	}
}
