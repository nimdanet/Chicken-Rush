using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour 
{
	public float vel;
	public float rotVel;
	private int rotDir;

	private Vector3 waypoint;

	private Transform myTransform;
	private Rigidbody myRigidbody;
	private Camera myCamera;

	private bool isFingerDown;
	private int terrainLayer;

	public Transform wpT;

	// Use this for initialization
	void Start () 
	{
		myRigidbody = rigidbody;
		myTransform = transform;
		myCamera = Camera.main;

		waypoint = Vector3.zero;

		isFingerDown = false;

		terrainLayer = 1 << LayerMask.NameToLayer ("Terrain");

		Input.simulateMouseWithTouches = true;
	}
	
	// Update is called once per frame
	void FixedUpdate () 
	{
		VerifyWaypoint ();

		if(waypoint != Vector3.zero)
		{
			//position
			float dist = Vector3.Distance (waypoint, myTransform.position);

			if(dist > 2f)
			{
				//rotation
				float angle = (Mathf.Atan2 (waypoint.z - myTransform.position.z, waypoint.x - myTransform.position.x) * Mathf.Rad2Deg) - 90f + PositiveAngle(myTransform.eulerAngles.y);
				rotDir = -(int)(NormalizeAngle(angle) / Mathf.Abs (NormalizeAngle(angle)));

				if(Mathf.Abs(NormalizeAngle(angle)) > rotVel)
					myTransform.Rotate(Vector3.up, rotVel * rotDir);


				//position
				angle = myTransform.eulerAngles.y;
				myRigidbody.velocity = myTransform.forward * vel;

				//myRigidbody.AddRelativeForce(myTransform.forward * vel);
			}
			else
			{
				waypoint = Vector3.zero;
				myRigidbody.velocity = Vector3.zero;
			}
		}
		else
		{
			waypoint = Vector3.zero;
			myRigidbody.velocity = Vector3.zero;
		}
	}

	private static float GetAngle(Vector2 v1, Vector2 v2)
	{
		var sign = Mathf.Sign(v1.x * v2.y - v1.y * v2.x);
		return Vector2.Angle(v1, v2) * sign;
	}

	void VerifyWaypoint()
	{
		if(GameController.Instance.inputMode == GameController.InputMode.Touch)
		{
			#if UNITY_ANDROID || UNITY_IPHONE
			if(Input.touchCount > 0)
			{
				Touch touch = Input.GetTouch(0);
				if(touch.phase == TouchPhase.Began) 
				{
					isFingerDown = true;

					Ray ray = myCamera.ScreenPointToRay((Vector3)touch.deltaPosition);
					RaycastHit hit;
					
					if(Physics.Raycast(ray, out hit, 100f, terrainLayer))
						SetWaypoint(hit.point);

				}
				if(touch.phase == TouchPhase.Ended) isFingerDown = false;

				if(isFingerDown)
				{
					if(touch.phase == TouchPhase.Moved)
					{
						Ray ray = myCamera.ScreenPointToRay((Vector3)touch.deltaPosition);
						RaycastHit hit;

						if(Physics.Raycast(ray, out hit, 100f, terrainLayer))
							SetWaypoint(hit.point);

					}
				}
			}

			#elif UNITY_EDITOR  ||  UNITY_WEBPLAYER
			if(Input.GetMouseButtonDown(0)) 
			{
				isFingerDown = true;
				
				Ray ray = myCamera.ScreenPointToRay(Input.mousePosition);
				RaycastHit hit;
				
				if(Physics.Raycast(ray, out hit, 100f, terrainLayer))
					SetWaypoint(hit.point);
				
			}
			if(Input.GetMouseButtonUp(0)) isFingerDown = false;
			
			if(isFingerDown)
			{
				Ray ray = myCamera.ScreenPointToRay(Input.mousePosition);
				RaycastHit hit;
				
				if(Physics.Raycast(ray, out hit, 100f, terrainLayer))
					SetWaypoint(hit.point);
				
			}
			#endif
		}
	}

	public void SetWaypoint(Vector3 point)
	{
		waypoint = point;
		wpT.position = point;

		//rotation
		Vector3 forward = myTransform.InverseTransformPoint(waypoint);
		float angle = (Mathf.Atan2 (waypoint.z - myTransform.position.z, waypoint.x - myTransform.position.x) * Mathf.Rad2Deg) - 90f + PositiveAngle(myTransform.eulerAngles.y);
		rotDir = -(int)(NormalizeAngle(angle) / Mathf.Abs (NormalizeAngle(angle)));

		//Debug.Log (PositiveAngle(myTransform.eulerAngles.y) + " & " + NormalizeAngle(angle) + " (" + angle + ")");
	}
	private static float NormalizeAngle(float angle)
	{
		if(angle < -180)
			angle = 360 + angle;

		if(angle > 180)
			angle = -(360 - angle);

		return angle;
	}

	private static float PositiveAngle(float angle)
	{
		while(angle > 360f)
			angle -= 360f;

		while(angle < 0)
			angle += 360f;

		return angle;
	}
}
