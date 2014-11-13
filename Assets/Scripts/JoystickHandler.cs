using UnityEngine;
using System.Collections;

public class JoystickHandler : MonoBehaviour 
{
	private Player player;
	private Transform joystickBase;
	private Transform joystickButton;

	private bool isTouching;

	private Vector2 screenSize;

	private Vector3 offset;
	private float buttonBaseSize;

	// Use this for initialization
	void Start () 
	{
		player = GameObject.FindWithTag ("Player").GetComponent<Player> ();

		joystickBase = transform.FindChild ("Base");
		joystickButton = transform.FindChild ("Button");

		joystickBase.GetComponent<UISprite> ().enabled = false;
		joystickButton.GetComponent<UISprite> ().enabled = false;

		UIRoot uiRoot = GameObject.Find ("UI Root").GetComponent<UIRoot> ();
		screenSize = new Vector2 (uiRoot.manualHeight * Camera.main.aspect, uiRoot.manualHeight);

		buttonBaseSize = joystickBase.GetComponent<UISprite> ().localSize.x / 2;
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(GameController.Instance.inputMode == GameController.InputMode.Joystick)
		{
			#if UNITY_ANDROID || UNITY_IPHONE


			#elif UNITY_EDITOR  ||  UNITY_WEBPLAYER

			if(Input.GetMouseButtonDown(0))
			{
				joystickBase.GetComponent<UISprite> ().enabled = true;
				joystickButton.GetComponent<UISprite> ().enabled = true;

				Vector3 pos = Camera.main.ScreenToViewportPoint(Input.mousePosition);
				pos.x *= screenSize.x;
				pos.y *= screenSize.y;

				offset = pos;

				transform.localPosition = pos;

				isTouching = true;
			}

			if(Input.GetMouseButtonUp(0)) 
			{
				joystickBase.GetComponent<UISprite> ().enabled = false;
				joystickButton.GetComponent<UISprite> ().enabled = false;

				player.SetWaypoint(Vector3.zero);

				isTouching = false;
			}

			if(isTouching)
			{
				Vector3 pos = Camera.main.ScreenToViewportPoint(Input.mousePosition);
				pos.x *= screenSize.x;
				pos.y *= screenSize.y;

				pos -= offset;

				joystickButton.localPosition = Vector3.ClampMagnitude(pos, buttonBaseSize);

				if(joystickButton.localPosition != Vector3.zero)
				{
					float angle = Mathf.Atan2(joystickButton.localPosition.y, joystickButton.localPosition.x);

					Vector3 waypoint = new Vector3(Mathf.Cos(angle), 0f, Mathf.Sin(angle)) * 3f;

					player.SetWaypoint(player.transform.position + waypoint);
				}
			}

			#endif
		}
	}
}
