using UnityEngine;
using System.Collections;

public class SpawnController : MonoBehaviour 
{
	public GameObject chicken;
	public Transform spawnPoints;
	private Transform player;

	private static SpawnController instance;
	public static SpawnController Instance
	{
		get { return instance; }
	}

	public float spawnTime = 1f;

	// Use this for initialization
	void Start () 
	{
		instance = this;
		player = GameObject.FindWithTag ("Player").transform;

		StartSpawn ();
	}

	public void StartSpawn()
	{
		StartCoroutine(SpawnChicken(spawnTime));
	}

	IEnumerator SpawnChicken(float waitTime)
	{
		yield return new WaitForSeconds(waitTime);

		GameObject obj = Instantiate (chicken) as GameObject;

		int index = Random.Range (0, spawnPoints.childCount);

		obj.transform.position = spawnPoints.GetChild (index).position;
		obj.transform.LookAt (player.position);
		obj.transform.eulerAngles = new Vector3 (0f, obj.transform.eulerAngles.y, obj.transform.eulerAngles.z);

		StartCoroutine(SpawnChicken(spawnTime));
	}
}
