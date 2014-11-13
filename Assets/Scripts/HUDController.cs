using UnityEngine;
using System.Collections;

public class HUDController : MonoBehaviour 
{
	public UILabel chickensDoged;
	public UILabel chickensHit;
	public UILabel time;


	private static HUDController instance;
	public static HUDController Instance
	{
		get { return instance; }
	}

	void Start()
	{
		instance = this;
	}

	public void RefreshPoints()
	{
		chickensDoged.text = GameController.chickensDodged.ToString();
		chickensHit.text = GameController.chickensHit.ToString();
	}

	public void RefreshTime()
	{
		int seconds = (int)GameController.time;
		int centiSeconds = (int)((GameController.time - Mathf.Floor(GameController.time)) * 100f);

		time.text = string.Format("{0:0}.{1:00}", seconds, centiSeconds);
	}
}
