using UnityEngine;
using System.Collections;

public class Pet : MonoBehaviour 
{
	protected Transform myTransform;
	protected Rigidbody myRigidbody;
	protected Transform player;

	public float vel;
	public float distanceFromPlayer;

	// Use this for initialization
	protected virtual void Start () 
	{
		myTransform = transform;
		myRigidbody = rigidbody;

		player = GameObject.FindWithTag ("Player").transform;
	}
	
	// Update is called once per frame
	protected virtual void Update () 
	{
		float dist = Vector3.Distance (myTransform.position, player.position);

		if(dist > distanceFromPlayer)
		{
			myTransform.LookAt (player);
			myTransform.eulerAngles = new Vector3 (0f, myTransform.eulerAngles.y, myTransform.eulerAngles.z);

			rigidbody.velocity = myTransform.forward * vel;
		}
		else
			rigidbody.velocity = Vector3.zero;
	}
}
