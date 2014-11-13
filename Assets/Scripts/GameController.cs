using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour 
{
	public enum InputMode
	{
		Touch,
		Joystick,
	}

	public static int chickensDodged;
	public static int chickensHit;

	public static float time;
	public InputMode inputMode = InputMode.Joystick;

	private static GameController instance;
	public static GameController Instance
	{
		get { return instance; }
	}
	
	void Start()
	{
		instance = this;

		chickensDodged = 0;
		chickensHit = 0;
		time = 0;
	}

	void Update()
	{
		time += Time.deltaTime;

		HUDController.Instance.RefreshTime ();
	}
}
